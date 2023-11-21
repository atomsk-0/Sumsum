using System.Runtime.InteropServices;
using System.Text;

namespace Proton.Network;

#pragma warning disable CS8500
#pragma warning disable CS8618

public static unsafe class ENet
{
    private const string EnetLib = "enet.dll";

    [StructLayout(LayoutKind.Explicit)]
    public struct ENetAddress
    {
        [FieldOffset(16)] public ushort port;
    }

    [Flags]
    public enum ENetPacketFlag
    {
        Reliable = 1 << 0,
        Unsequenced = 1 << 1,
        NoAllocate = 1 << 2,
        UnreliableFragment = 1 << 3,
        Sent = 1 << 8
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ENetPacket
    {
        public UIntPtr referenceCount; //0x0
        public uint flags;             //0x8
        public IntPtr data;            //0xc
        public UIntPtr dataLength;     //0x14
        public IntPtr freeCallback;    //0x1c
        public IntPtr userData;        //0x24

        public Span<byte> GetDataAsSpan() => new(data.ToPointer(), unchecked((int)dataLength));

        public byte* Data() => (byte*)data.ToPointer();

        public int GetDataLength() => unchecked((int)dataLength);
    }

    public enum ENetPeerState
    {
        Disconnected = 0,
        Connecting = 1,
        AcknowledgingConnect = 2,
        ConnectionPending = 3,
        ConnectionSucceeded = 4,
        Connected = 5,
        DisconnectLater = 6,
        Disconnecting = 7,
        AcknowledgingDisconnect = 8,
        Zombie = 9
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ENetBuffer
    {
        public UIntPtr dataLength;
        public IntPtr data;
    }


    public enum ENetEventType
    {
        None = 0,
        Connect = 1,
        Disconnect = 2,
        Receive = 3,
        DisconnectTimeout = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ENetEvent
    {
        public ENetEventType type;
        public IntPtr peer;
        public byte channelID;
        public uint data;
        public ENetPacket* packet;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ENetVersion
    {
        public byte major;
        public byte minor;
        public byte patch;
    }


    [DllImport(EnetLib)]
    public static extern int enet_initialize();

    [DllImport(EnetLib)]
    public static extern void enet_deinitialize();

    [DllImport(EnetLib)]
    public static extern ENetVersion enet_linked_version();

    [DllImport(EnetLib)]
    public static extern uint enet_time_get();

    [DllImport(EnetLib)]
    public static extern int enet_address_set_host_ip_old(ref ENetAddress address, string hostName);

    [DllImport(EnetLib)]
    public static extern int enet_address_set_host_old(ref ENetAddress address, string hostName);

    [DllImport(EnetLib)]
    public static extern int enet_address_get_host_ip_old(ref ENetAddress address, [MarshalAs(UnmanagedType.LPStr)] StringBuilder hostName, nuint nameLength);

    [DllImport(EnetLib)]
    public static extern int enet_address_get_host_old(ref ENetAddress address, [MarshalAs(UnmanagedType.LPStr)] StringBuilder hostName, nuint nameLength);

    [DllImport(EnetLib)]
    public static extern int enet_address_set_host_ip(ref ENetAddress address, string hostName);

    [DllImport(EnetLib)]
    public static extern int enet_address_set_host(ENetAddress* address, string hostName);

    [DllImport(EnetLib)]
    public static extern int enet_address_get_host_ip(ref ENetAddress address, [MarshalAs(UnmanagedType.LPStr)] StringBuilder hostName, nuint nameLength);

    [DllImport(EnetLib)]
    public static extern int enet_address_get_host(ref ENetAddress address, [MarshalAs(UnmanagedType.LPStr)] StringBuilder hostName, nuint nameLength);

    [DllImport(EnetLib)]
    public static extern uint enet_host_get_peers_count(nint host);

    [DllImport(EnetLib)]
    public static extern uint enet_host_get_packets_sent(nint host);

    [DllImport(EnetLib)]
    public static extern uint enet_host_get_packets_received(nint host);

    [DllImport(EnetLib)]
    public static extern uint enet_host_get_bytes_sent(nint host);

    [DllImport(EnetLib)]
    public static extern uint enet_host_get_bytes_received(nint host);

    [DllImport(EnetLib)]
    public static extern uint enet_host_get_received_data(nint host, nint* data);

    [DllImport(EnetLib)]
    public static extern uint enet_host_get_mtu(nint host);

    [DllImport(EnetLib)]
    public static extern uint enet_peer_get_id(nint peer);

    [DllImport(EnetLib)]
    public static extern uint enet_peer_get_ip(nint peer, [MarshalAs(UnmanagedType.LPStr)] StringBuilder ip, nuint ipLength);

    [DllImport(EnetLib)]
    public static extern ushort enet_peer_get_port(nint peer);

    [DllImport(EnetLib)]
    public static extern uint enet_peer_get_rtt(nint peer);

    [DllImport(EnetLib)]
    public static extern ulong enet_peer_get_packets_sent(nint peer);

    [DllImport(EnetLib)]
    public static extern uint enet_peer_get_packets_lost(nint peer);

    [DllImport(EnetLib)]
    public static extern ulong enet_peer_get_bytes_sent(nint peer);

    [DllImport(EnetLib)]
    public static extern ulong enet_peer_get_bytes_received(nint peer);

    [DllImport(EnetLib)]
    public static extern ENetPeerState enet_peer_get_state(nint peer);

    [DllImport(EnetLib)]
    public static extern nint enet_peer_get_data(nint peer);

    [DllImport(EnetLib)]
    public static extern void enet_peer_set_data(nint peer, nint data);

    [DllImport(EnetLib)]
    public static extern nint enet_packet_get_data(ENetPacket packet);

    [DllImport(EnetLib)]
    public static extern uint enet_packet_get_length(ENetPacket packet);

    [DllImport(EnetLib)]
    public static extern void enet_packet_set_free_callback(ENetPacket packet, nint callback);

    [DllImport(EnetLib)]
    public static extern ENetPacket* enet_packet_create_offset(nint data, nuint dataLength, nuint dataOffset, uint flags);

    [DllImport(EnetLib)]
    public static extern uint enet_crc32([MarshalAs(UnmanagedType.LPArray)] nint buffers, nuint bufferCount);

    [DllImport(EnetLib)]
    public static extern nint enet_host_create(ENetAddress* address, nuint peerLimit, nuint channelLimit, uint incomingBandwidth, uint outgoingBandwidth);

    [DllImport(EnetLib)]
    public static extern void enet_host_destroy(nint host);

    [DllImport(EnetLib)]
    public static extern nint enet_host_connect(nint host, ENetAddress* address, nuint channelCount, uint data);

    [DllImport(EnetLib)]
    public static extern int enet_host_check_events(nint host, ENetEvent* netEvent);

    [DllImport(EnetLib)]
    public static extern int enet_host_service(nint host, ENetEvent* netEvent, uint timeout);

    [DllImport(EnetLib)]
    public static extern int enet_host_send_raw(nint host, ENetAddress* address, byte* data, nuint dataLength);

    [DllImport(EnetLib)]
    public static extern int enet_host_send_raw_ex(nint host, ENetAddress* address, byte* data, nuint skipBytes, nuint bytesToSend);

    [DllImport(EnetLib)]
    public static extern void enet_host_flush(nint host);

    [DllImport(EnetLib)]
    public static extern void enet_host_broadcast(nint host, byte channelId, ENetPacket* packet);

    [DllImport(EnetLib)]
    public static extern void enet_host_channel_limit(nint host, nuint channelLimit);

    [DllImport(EnetLib)]
    public static extern void enet_host_bandwidth_limit(nint host, uint incomingBandwidth, uint outgoingBandwidth);

    [DllImport(EnetLib)]
    public static extern void enet_host_bandwidth_throttle(nint host);

    [DllImport(EnetLib)]
    public static extern ulong enet_host_random_seed();

    [DllImport(EnetLib)]
    public static extern int enet_peer_send(nint peer, byte channelId, ENetPacket* packet);

    [DllImport(EnetLib)]
    public static extern ENetPacket* enet_peer_receive(nint peer, byte* channelId);

    [DllImport(EnetLib)]
    public static extern void enet_peer_ping(nint peer);

    [DllImport(EnetLib)]
    public static extern void enet_peer_ping_interval(nint peer, uint pingInterval);

    [DllImport(EnetLib)]
    public static extern void enet_peer_timeout(nint peer, uint timeoutLimit, uint timeoutMinimum, uint timeoutMaximum);

    [DllImport(EnetLib)]
    public static extern void enet_peer_reset(nint peer);

    [DllImport(EnetLib)]
    public static extern void enet_peer_disconnect(nint peer, uint data);

    [DllImport(EnetLib)]
    public static extern void enet_peer_disconnect_now(nint peer, uint data);

    [DllImport(EnetLib)]
    public static extern void enet_peer_disconnect_later(nint peer, uint data);

    [DllImport(EnetLib)]
    public static extern void enet_peer_throttle_configure(nint peer, uint interval, uint acceleration, uint deceleration);

    [DllImport(EnetLib)]
    public static extern void enet_host_use_crc32(nint host);

    [DllImport(EnetLib)]
    public static extern void enet_packet_destroy(ENetPacket* packet);

    [DllImport(EnetLib)]
    public static extern int enet_host_compress_with_range_coder(nint host);

    [DllImport(EnetLib)]
    public static extern void enet_host_use_new_packet(nint host);

    [DllImport(EnetLib)]
    public static extern void enet_host_use_proxy(nint host, [MarshalAs(UnmanagedType.LPStr)] string ip, ushort port, [MarshalAs(UnmanagedType.LPStr)] string username, [MarshalAs(UnmanagedType.LPStr)] string password);

    [DllImport(EnetLib)]
    public static extern ENetPacket* enet_packet_create(nint data, nuint dataLength, ENetPacketFlag flags);

    [DllImport(EnetLib)]
    public static extern uint enet_peer_get_ping(nint peer);
}