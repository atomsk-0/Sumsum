using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Sumsum.Internal.Util.WinApi;
using Console = Sumsum.Internal.Util.Console;

namespace Sumsum.Internal;

internal static unsafe class Main
{
    private const uint DllProcessDetach = 0, DllProcessAttach = 1, DllThreadAttach = 2, DllThreadDetach = 3;
    
    [UnmanagedCallersOnly(EntryPoint = "DllMain", CallConvs = new[] { typeof(CallConvStdcall) })]
    public static bool DllMain(IntPtr hModule, uint ulReasonForCall, IntPtr lpReserved)
    {
        if (ulReasonForCall == DllProcessAttach)
        {
            DisableThreadLibraryCalls(hModule);
            CreateThread(null, 0, OnInjected, null, 0, out _);
        }
        return true;
    }

    private static void OnInjected()
    {
        Console.Setup();
        Console.WriteLine("Console initialized!");
    }
}