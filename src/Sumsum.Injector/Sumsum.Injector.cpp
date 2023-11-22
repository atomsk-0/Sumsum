//Injector from venom

#include <iostream>
#include <array>
#include <filesystem>
#include <windows.h>
#include <shlobj.h>

typedef NTSTATUS(_stdcall* _NtSetInformationProcess)(
	HANDLE           ProcessHandle,
	PROCESS_INFORMATION_CLASS ProcessInformationClass,
	PVOID            ProcessInformation,
	ULONG            ProcessInformationLength
	);

// from https://blog.sevagas.com/IMG/pdf/code_injection_series_part4.pdf
BOOL SetRemoteProcessMitigationPolicy(
	DWORD targetPid,
	PROCESS_MITIGATION_POLICY MitigationPolicy,
	PVOID lpBuffer,
	SIZE_T dwLength) {

	BOOL result = FALSE;
	HANDLE proc = OpenProcess(PROCESS_ALL_ACCESS | PROCESS_SET_INFORMATION, FALSE, targetPid);

	if (proc != NULL) {
		_NtSetInformationProcess NtSetInformationProcess = (_NtSetInformationProcess)GetProcAddress(GetModuleHandle("ntdll.dll"), "NtSetInformationProcess");

		uint64_t policy = *(DWORD*)lpBuffer;
		policy = policy << 32;
		policy += (DWORD)MitigationPolicy;

		NTSTATUS ret = NtSetInformationProcess(
			proc,
			(PROCESS_INFORMATION_CLASS)0x34,
			&policy,
			sizeof(policy)
		);

		if (ret == 0)
			result = TRUE;

		CloseHandle(proc);
	}

	return result;
}

BOOL EnableDebugPrivilege() {
	HANDLE token;
	TOKEN_PRIVILEGES tp;
	BOOL result;
	LUID luid;

	result = OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &token);
	if (!result) {
		return FALSE;
	}

	result = LookupPrivilegeValue(NULL, SE_DEBUG_NAME, &luid);
	if (!result) {
		CloseHandle(token);
		return FALSE;
	}

	tp.PrivilegeCount = 1;
	tp.Privileges[0].Luid = luid;
	tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;

	result = AdjustTokenPrivileges(token, FALSE, &tp, sizeof(TOKEN_PRIVILEGES), NULL, NULL);
	if (!result) {
		CloseHandle(token);
		return FALSE;
	}

	CloseHandle(token);
	return TRUE;
}

int main(int argc, char** argv) {
	if (argc != 2) {
		std::cerr << "usage: drop dll on top of this executable\n";
		std::cin.get();
		return 1;
	}

	if (!EnableDebugPrivilege()) {
		std::cerr << "failed to enable debug privilege\n";
		std::cin.get();
		return 1;
	}

	char path[MAX_PATH];
	SHGetFolderPath(NULL, CSIDL_COMMON_DOCUMENTS, NULL, 0, path);
	std::strcat(path, "\\Growtopia 4.19\\Growtopia.exe");

	if (!std::filesystem::exists(path))
	{
		std::memset(path, 0, sizeof(path));
		SHGetFolderPath(NULL, CSIDL_LOCAL_APPDATA, NULL, 0, path);
		std::strcat(path, "\\Growtopia\\Growtopia.exe");
	}
	
	STARTUPINFO startup_info = {};
	PROCESS_INFORMATION process_info = {};
	if (!CreateProcess(path, NULL, NULL, NULL, FALSE, 0, NULL, NULL, &startup_info, &process_info)) {
		std::cerr << "failed to start growtopia.exe\n";
		std::cin.get();
		return 1;
	}

	WaitForInputIdle(process_info.hProcess, INFINITE);

	HWND hwnd = FindWindow(NULL, "Growtopia");
	if (hwnd == NULL) {
		std::cerr << "failed to find growtopia window\n";
		std::cin.get();
		return 1;
	}

	DWORD process_id;
	GetWindowThreadProcessId(hwnd, &process_id);

	PROCESS_MITIGATION_DYNAMIC_CODE_POLICY policy = {};
	policy.ProhibitDynamicCode = FALSE;
	if (!SetRemoteProcessMitigationPolicy(
		process_id,
		ProcessDynamicCodePolicy,
		&policy,
		sizeof(policy)
	)) {
		std::cerr << "failed to disable BlockDynamicCode mitigation policy\n";
		std::cin.get();
		return 1;
	}

	const char* dll_path = argv[1];
	std::size_t dll_path_len = std::strlen(dll_path);

	HMODULE module_handle = LoadLibrary(dll_path);
	if (module_handle == NULL) {
		std::cerr << "failed to load dll\n";
		std::cin.get();
		return 1;
	}

	HANDLE process_handle = OpenProcess(PROCESS_ALL_ACCESS, FALSE, process_id);

	// restore NtProtectVirtualMemory original bytes
	constexpr std::array<std::uint8_t, 5> bytes = { 0x4C, 0x8B, 0xD1, 0xB8, 0x50 };
	if (!WriteProcessMemory(
		process_handle,
		GetProcAddress(GetModuleHandle("ntdll.dll"), "NtProtectVirtualMemory"),
		bytes.data(),
		bytes.size(),
		nullptr
	)) {
		std::cerr << "failed to restore NtProtectVirtualMemory original bytes\n";
		std::cin.get();
		return 1;
	}

	// inject dll
	LPVOID remote_address = VirtualAllocEx(process_handle, NULL, dll_path_len + 1, MEM_COMMIT, PAGE_READWRITE);
	WriteProcessMemory(process_handle, remote_address, dll_path, dll_path_len + 1, NULL);
	LPTHREAD_START_ROUTINE thread_start_routine = (LPTHREAD_START_ROUTINE)GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
	HANDLE remote_thread = CreateRemoteThread(process_handle, NULL, 0, thread_start_routine, remote_address, 0, NULL);
	WaitForSingleObject(remote_thread, INFINITE);
	CloseHandle(remote_thread);
	VirtualFreeEx(process_handle, remote_address, dll_path_len + 1, MEM_RELEASE);
	CloseHandle(process_handle);
	
	return 0;
}