using System.Runtime.InteropServices;

namespace Sumsum.Internal.Util;

#pragma warning disable CS0652 // Comparison to integral constant is useless; the constant is outside the range of the type

public static class Memory
{
    public static IntPtr BaseAddress;
    public static IntPtr EndAddress;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

    private static List<byte> StringToBytes(string str)
    {
        List<byte> bytes = new List<byte>();

        char[] chars = str.ToCharArray();

        for (int i = 0; i < chars.Length;)
        {
            if (chars[i] == ' ')
            {
                i++;
                continue;
            }

            bytes.Add(Convert.ToByte(chars[i].ToString() + chars[i + 1].ToString(), 16));
            i += 2;
        }

        return bytes;
    }

    private static List<int> PatternToBytes(string pattern)
    {
        List<int> bytes = new List<int>();

        pattern = pattern.Replace(" ", "");

        if (pattern.Length % 2 != 0)
        {
            throw new ArgumentException("Invalid pattern length. Must be a multiple of 2.");
        }

        for (int i = 0; i < pattern.Length; i += 2)
        {
            string hexPair = pattern.Substring(i, 2);

            if (hexPair == "??")
            {
                bytes.Add(-1);
            }
            else
            {
                bytes.Add(Convert.ToInt32(hexPair, 16));
            }
        }

        return bytes;
    }


    public static bool WriteBytes(IntPtr address, string stringBytes)
    {
        List<byte> bytes = StringToBytes(stringBytes);
        IntPtr dest = address;

        if (!VirtualProtect(dest, (uint)bytes.Count, 0x40, out var oldProtection)) return false;

        Marshal.Copy(bytes.ToArray(), 0, dest, bytes.Count);

        return VirtualProtect(dest, (uint)bytes.Count, oldProtection, out oldProtection);
    }

    public static IntPtr FindPattern(string pattern, int offset = 0)
    {
        if (BaseAddress == IntPtr.Zero || EndAddress == IntPtr.Zero) return IntPtr.Zero;

        List<int> bytes = PatternToBytes(pattern);

        for (IntPtr i = BaseAddress; i.ToInt64() < EndAddress.ToInt64(); i = IntPtr.Add(i, 1))
        {
            for (int j = 0; j < bytes.Count; j++)
            {
                if (Marshal.ReadByte(i + j) != bytes[j] && bytes[j] != -1)
                    break;

                if (j == bytes.Count - 1)
                    return IntPtr.Add(i, offset);
            }
        }

        return IntPtr.Zero;
    }

    public static IntPtr GetAddressFromCall(IntPtr address)
    {
        if (address == IntPtr.Zero)
            return IntPtr.Zero;

        if (Marshal.ReadByte(address) != 0xE8)
            return IntPtr.Zero;

        int relativeAddress = Marshal.ReadInt32(address + 1);

        return IntPtr.Add(address, relativeAddress + sizeof(int) + 1);
    }

    public static IntPtr GetAddressFromLoad(IntPtr address)
    {
        if (address == IntPtr.Zero)
            return IntPtr.Zero;

        int relativeAddress = Marshal.ReadInt32(address);

        return IntPtr.Add(address, relativeAddress + sizeof(int));
    }

    public static IntPtr FindFunctionStart(IntPtr address)
    {
        if (address == IntPtr.Zero)
            return IntPtr.Zero;
        
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        while (Marshal.ReadInt16(address - 2) != 0xCCCC)
            address = IntPtr.Subtract(address, 1);

        return address;
    }
    
    public enum FindMode
    {
        Normal,
        Call,
        Load,
        FunctionStart
    }
    
    public static void FindAddress(out IntPtr dest, string pattern, FindMode mode, int offset = 0)
    {
        var address = FindPattern(pattern, offset);

        switch (mode)
        {
            case FindMode.Call:
                address = GetAddressFromCall(address);
                break;

            case FindMode.Load:
                address = GetAddressFromLoad(address);
                break;

            case FindMode.FunctionStart:
                address = FindFunctionStart(address);
                break;
        }

        if (address == IntPtr.Zero)
        {
            Log.Error($"Pattern '{pattern}' not found");
        }

        dest = address;
    }
}