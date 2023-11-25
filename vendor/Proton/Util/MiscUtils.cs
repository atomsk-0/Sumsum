namespace Proton.Util;

public static class MiscUtils
{
    public static uint HashBytes(IEnumerable<byte> b) => b.Aggregate<byte, uint>(0x55555555, (current, t) => (current >> 27) + (current << 5) + t);

    public static bool IsEven(int number) => (number & 1) == 0;
}