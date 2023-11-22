using System.Runtime.InteropServices;

namespace Sumsum.Internal.Util;

public static class WinApi
{
    public const int STD_OUTPUT_HANDLE = -11;
    const int STD_INPUT_HANDLE = -10;
    public const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
    
    const ushort KEY_EVENT = 1;

    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT_RECORD
    {
        [FieldOffset(0)]
        public ushort EventType;
        [FieldOffset(4)]
        public KEY_EVENT_RECORD KeyEvent;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct KEY_EVENT_RECORD
    {
        [FieldOffset(0)]
        public bool bKeyDown;
        [FieldOffset(4)]
        public ushort wRepeatCount;
        [FieldOffset(6)]
        public ushort wVirtualKeyCode;
        [FieldOffset(8)]
        public ushort wVirtualScanCode;
        [FieldOffset(10)]
        public char UnicodeChar;
        [FieldOffset(12)]
        public uint dwControlKeyState;
    }
    
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetCurrentProcess();

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetModuleHandle(string moduleName);

    [DllImport("psapi.dll", SetLastError = true)]
    public static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out MODULEINFO lpmodinfo, uint cb);

    [StructLayout(LayoutKind.Sequential)]
    public struct MODULEINFO
    {
        public IntPtr lpBaseOfDll;
        public uint SizeOfImage;
        public IntPtr EntryPoint;
    }
    
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleTitle(string lpConsoleTitle);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteConsole(IntPtr hConsoleOutput, string lpBuffer, uint nNumberOfCharsToWrite, out uint lpNumberOfCharsWritten, IntPtr lpReserved);

    [DllImport("kernel32.dll")]
    public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, ushort wAttributes);
    
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadConsoleInput(IntPtr hConsoleInput, out INPUT_RECORD lpBuffer, uint nLength, out uint lpNumberOfEventsRead);

    [DllImport("kernel32.dll", SetLastError=true)]
    [PreserveSig]
    public static extern bool DisableThreadLibraryCalls
    (
        [In]
        IntPtr hModule
    );
    
    [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern unsafe uint CreateThread(
        uint* lpThreadAttributes,
        uint dwStackSize,
        ThreadStart lpStartAddress,
        uint* lpParameter,
        uint dwCreationFlags,
        out uint lpThreadId);
}
    