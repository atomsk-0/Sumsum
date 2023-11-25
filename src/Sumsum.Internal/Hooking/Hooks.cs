using System.Runtime.InteropServices;
using MinSharp;
using Sumsum.Internal.Growtopia.Entities;
using Sumsum.Internal.Growtopia.Network;
using Sumsum.Internal.Util;

namespace Sumsum.Internal.Hooking;

public static unsafe class Hooks
{
   private static void HookFunction<TDelegate>(ref TDelegate function, TDelegate hookFunction) where TDelegate : Delegate
   {
      var address = Marshal.GetFunctionPointerForDelegate(function);

      var hook = new Hook<TDelegate>(address, hookFunction);
      hook.Enable();

      function = hook.Original;
   }
   
   public static void Setup()
   {
      HookFunction(ref ENetClient.SendPacket!, SendPacketHook);
   }

   private static void SendPacketHook(NetMessageType type, string packet, void* peer)
   {
      Log.Info($"Client sent packet: {type}");
      Log.Info($"Packet Content: {packet}"); //Login packets seems to be corrupted not sure why
      //ENetClient.SendPacket?.Invoke(type, packet, peer); Not sending the packet because of the corruption
   }
}

