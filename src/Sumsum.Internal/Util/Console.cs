using static Sumsum.Internal.Util.WinApi;

namespace Sumsum.Internal.Util;

public static class Console
{
    public enum ForegroundColor : ushort
    {
        Black = 0x0000,
        DarkBlue = 0x0001,
        DarkGreen = 0x0002,
        DarkCyan = 0x0003,
        DarkRed = 0x0004,
        DarkMagenta = 0x0005,
        DarkYellow = 0x0006,
        Gray = 0x0007,
        DarkGray = 0x0008,
        Blue = 0x0009,
        Green = 0x000A,
        Cyan = 0x000B,
        Red = 0x000C,
        Magenta = 0x000D,
        Yellow = 0x000E,
        White = 0x000F
    }
    
    public static void Setup()
    {
        AllocConsole();
        IntPtr console = GetStdHandle(STD_OUTPUT_HANDLE);
        GetConsoleMode(console, out var mode);
        SetConsoleMode(console, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
        SetConsoleTitle("Sensum Internal");
        SetConsoleTextAttribute(console, (ushort)ForegroundColor.White);
        Log.Info("Console initialized");
    }
    
    public static void Write(string message)
    {
        IntPtr console = GetStdHandle(STD_OUTPUT_HANDLE);
        WriteConsole(console, message, (uint)message.Length, out _, IntPtr.Zero);
    }
    
    public static void Write(string message, ForegroundColor color)
    {
        IntPtr console = GetStdHandle(STD_OUTPUT_HANDLE);
        SetConsoleTextAttribute(console, (ushort)color);
        WriteConsole(console, message, (uint)message.Length, out _, IntPtr.Zero);
        SetConsoleTextAttribute(console, (ushort)ForegroundColor.White);
    }
    
    public static void WriteLine(string message)
    {
        message += "\n";
        Write(message);
    }
    
    public static void WriteLine(string message, ForegroundColor color)
    {
        message += "\n";
        Write(message, color);
    }
    
    public static void ReadKey()
    {
        IntPtr consoleInput = GetStdHandle(STD_INPUT_HANDLE);
        INPUT_RECORD inputRecord;
        uint eventsRead;

        while (true)
        {
            ReadConsoleInput(consoleInput, out inputRecord, 1, out eventsRead);

            if (eventsRead > 0 && inputRecord.EventType == KEY_EVENT && inputRecord.KeyEvent.bKeyDown)
            {
                char keyChar = inputRecord.KeyEvent.UnicodeChar;
                IntPtr console = GetStdHandle(STD_OUTPUT_HANDLE);
                WriteConsole(console, $"Key pressed: {keyChar}", (uint)$"Key pressed: {keyChar}".Length, out _, IntPtr.Zero);
                break;
            }
        }
    }
}