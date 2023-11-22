namespace Sumsum.Internal.Util;

public static class Log
{
    public static void Info(string message)
    {
        Console.Write("[+]: ", Console.ForegroundColor.Blue);
        Console.WriteLine(message);
    }
    
    public static void Error(string message)
    {
        Console.Write("[-]: ", Console.ForegroundColor.Red);
        Console.WriteLine(message);
    }
    
    public static void Debug(string message)
    {
        Console.Write("[#]: ", Console.ForegroundColor.DarkYellow);
        Console.WriteLine(message);
    }
}