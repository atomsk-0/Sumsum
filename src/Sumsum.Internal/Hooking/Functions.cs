using System.Runtime.InteropServices;
using Sumsum.Internal.Growtopia.App;
using Sumsum.Internal.Growtopia.Entities;
using Sumsum.Internal.Growtopia.Network;
using Sumsum.Internal.Util;

namespace Sumsum.Internal.Hooking;

public static class Functions
{
    private static TDelegate GetFunction<TDelegate>(string pattern, Memory.FindMode mode, int offset = 0)
    {
        Memory.FindAddress(out var addr, pattern, mode, offset);
        return Marshal.GetDelegateForFunctionPointer<TDelegate>(addr);
    }
    
    internal static void GetFunctions()
    {
        BaseApp.SetFpsLimit = GetFunction<SetFpsLimitDelegate>("E8 E5 6F F2 FF", Memory.FindMode.Call);
        ENetClient.SendPacket = GetFunction<SendPacketDelegate>("02 00 00 00 E8 F8 8A 13 00", Memory.FindMode.Call, 4);
    }
}

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void SetFpsLimitDelegate(float fps);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate void SendPacketDelegate(NetMessageType type, string packet, void* peer);