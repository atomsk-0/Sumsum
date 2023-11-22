using static Sumsum.Internal.Util.WinApi;

namespace Sumsum.Internal.Util;

public static class Console
{
    public static void Setup()
    {
        AllocConsole();
        IntPtr console = GetStdHandle(STD_OUTPUT_HANDLE);
        GetConsoleMode(console, out var mode);
        SetConsoleMode(console, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
        SetConsoleTitle("Sensum Internal");
            
    }
        
    public static void WriteLine(string message)
    {
        message += "\n";
        IntPtr console = GetStdHandle(STD_OUTPUT_HANDLE);
        WriteConsole(console, message, (uint)message.Length, out _, IntPtr.Zero);
    }
}