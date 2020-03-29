using Pluspy.Enums;

namespace Pluspy.Net.Packets.Requests
{
    public readonly struct OpenHorseWindowRequestPacket : IRequestPacket
    {
        public byte Id => 0x20;
        
        public byte WindowID { get; }
        public int NumberOfSlots { get; }
        public int EntityID { get; }

        public OpenHorseWindowRequestPacket(byte windowId, int numberOfSlots, int entityId)
        {
            WindowID = windowId;
            NumberOfSlots = numberOfSlots;
            EntityID = entityId;
        }
        
        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.Write(WindowID);
            stream.Write(NumberOfSlots);
            stream.Write(EntityID);
            return state;
        }
    }
}