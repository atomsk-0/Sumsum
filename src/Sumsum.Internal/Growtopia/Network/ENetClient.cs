using Sumsum.Internal.Hooking;

namespace Sumsum.Internal.Growtopia.Network;

public static class ENetClient
{
    /* SendPacket(eNetMessageType, std::basic_string<char, std::char_traits<char>, std::allocator<char>> const&, _ENetPeer*) */
    public static SendPacketDelegate? SendPacket;
}