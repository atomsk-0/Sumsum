using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Sumsum.Internal.Growtopia;
using static Sumsum.Internal.Util.WinApi;
using Console = Sumsum.Internal.Util.Console;

namespace Sumsum.Internal;

internal static unsafe class Main
{
    internal static IntPtr Module = IntPtr.Zero;
    
    private const uint DllProcessDetach = 0, DllProcessAttach = 1, DllThreadAttach = 2, DllThreadDetach = 3;
    
    [UnmanagedCallersOnly(EntryPoint = "DllMain", CallConvs = new[] { typeof(CallConvStdcall) })]
    
    // ReSharper disable once RedundantAssignment
    public static bool DllMain(IntPtr hModule, uint ulReasonForCall, IntPtr lpReserved)
    {
        if (ulReasonForCall == DllProcessAttach)
        {
            hModule = Module;
            DisableThreadLibraryCalls(hModule);
            CreateThread(null, 0, OnInjected, null, 0, out _);
        }
        return true;
    }

    private static void OnInjected()
    {
        Console.Setup();
        Game.Setup();
    }
}