using System.Text;
// ReSharper disable All

namespace Sumsum.Internal.Growtopia.Entities;

public struct GameUpdatePacket
{
    public NetGamePacketType Type; // Offset: 0x0 (0 in decimal)
    public byte Padding1;          // Offset: 0x1 (1 in decimal)
    public ushort Padding2;        // Offset: 0x2 (2 in decimal)
    public int NetId;              // Offset: 0x4 (4 in decimal)
    public int SecondaryNetId;     // Offset: 0x8 (8 in decimal)
    public int CharacterState;     // Offset: 0xC (12 in decimal)
    public float Flags;            // Offset: 0x10 (16 in decimal)
    public int Value;              // Offset: 0x14 (20 in decimal)
    public float WorldPosX;        // Offset: 0x18 (24 in decimal)
    public float WorldPosY;        // Offset: 0x1C (28 in decimal)
    public int SpeedX;             // Offset: 0x20 (32 in decimal)
    public int SpeedY;             // Offset: 0x24 (36 in decimal)
    public int Padding3;           // Offset: 0x28 (40 in decimal)
    public int TileX;              // Offset: 0x2C (44 in decimal)
    public int TileY;              // Offset: 0x30 (48 in decimal)
    public int ExtraDataSize;      // Offset: 0x34 (52 in decimal)
    
    public override string ToString()
    {
        return new StringBuilder().AppendLine($"Type: {Type}").AppendLine($"Padding1: {Padding1}")
            .AppendLine($"Padding2: {Padding2}").AppendLine($"NetId: {NetId}").AppendLine($"SecondaryNetId: {SecondaryNetId}")
            .AppendLine($"CharacterState: {CharacterState}").AppendLine($"Flags: {Flags}").AppendLine($"Value: {Value}")
            .AppendLine($"WorldPos: {WorldPosX}:{WorldPosY}").AppendLine($"Speed: {SpeedX}:{SpeedY}").AppendLine($"Padding3: {Padding3}")
            .AppendLine($"TilePos: {TileX}:{TileY}").AppendLine($"ExtraDataSize: {ExtraDataSize}").ToString();
    }
}

public enum NetMessageType : uint
{
    Unknown = 0, // Hex[0x0], Decimal[0]
    ServerHello, // Hex[0x1], Decimal[1]
    GenericText, // Hex[0x2], Decimal[2]
    GameMessage, // Hex[0x3], Decimal[3]
    GamePacket, // Hex[0x4], Decimal[4]
    Error, // Hex[0x5], Decimal[5]
    Track, // Hex[0x6], Decimal[6]
    LogRequest, // Hex[0x7], Decimal[7]
    LogResponse // Hex[0x8], Decimal[8]
}

public enum NetGamePacketType : byte
{
    State = 0, // Hex[0x0], Decimal[0]
    CallFunction, // Hex[0x1], Decimal[1]
    UpdateStatus, // Hex[0x2], Decimal[2]
    TileChangeRequest, // Hex[0x3], Decimal[3]
    SendMapData, // Hex[0x4], Decimal[4]
    SendTileUpdateData, // Hex[0x5], Decimal[5]
    SendTileUpdateDataMultiple, // Hex[0x6], Decimal[6]
    TileActivateRequest, // Hex[0x7], Decimal[7]
    TileApplyDamage, // Hex[0x8], Decimal[8]
    SendInventoryState, // Hex[0x9], Decimal[9]
    ItemActivateRequest, // Hex[0xA], Decimal[10]
    ItemActivateObjectRequest, // Hex[0xB], Decimal[11]
    SendTileTreeState, // Hex[0xC], Decimal[12]
    ModifyItemInventory, // Hex[0xD], Decimal[13]
    ItemChangeObject, // Hex[0xE], Decimal[14]
    SendLock, // Hex[0xF], Decimal[15]
    SendItemDatabaseData, // Hex[0x10], Decimal[16]
    SendParticleEffect, // Hex[0x11], Decimal[17]
    SetIconState, // Hex[0x12], Decimal[18]
    ItemEffect, // Hex[0x13], Decimal[19]
    SetCharacterState, // Hex[0x14], Decimal[20]
    PingReply, // Hex[0x15], Decimal[21]
    PingRequest, // Hex[0x16], Decimal[22]
    GotPunched, // Hex[0x17], Decimal[23]
    AppCheckResponse, // Hex[0x18], Decimal[24]
    AppIntegrityFail, // Hex[0x19], Decimal[25]
    Disconnect, // Hex[0x1A], Decimal[26]
    BattleJoin, // Hex[0x1B], Decimal[27]
    BattleEven, // Hex[0x1C], Decimal[28]
    UseDoor, // Hex[0x1D], Decimal[29]
    SendParental, // Hex[0x1E], Decimal[30]
    GoneFishin, // Hex[0x1F], Decimal[31]
    Steam, // Hex[0x20], Decimal[32]
    PetBattle, // Hex[0x21], Decimal[33]
    Npc, // Hex[0x22], Decimal[34]
    Special, // Hex[0x23], Decimal[35]
    SendParticleEffectV2, // Hex[0x24], Decimal[36]
    ActiveArrowToItem, // Hex[0x25], Decimal[37]
    SelectTileIndex, // Hex[0x26], Decimal[38]
    SendPlayerTributeData, // Hex[0x27], Decimal[39]
    SetExtraMods, // Hex[0x28], Decimal[40]
    OnStepOnTileMod // Hex[0x29], Decimal[41]
}