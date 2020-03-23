using Pluspy.Enums;

namespace Pluspy.Net
{
    public sealed class NetworkService
    {
        private readonly MinecraftStream _stream;

        public State CurrentState { get; private set; }

        public NetworkService(MinecraftStream stream)
        {
            _stream = stream;
        }

        public void WritePacket<TPacket>(TPacket packet) where TPacket : struct, IRequestPacket
        {
            _stream.WriteVarInt(packet.Length);
            _stream.WriteByte(packet.Id);
            CurrentState = packet.WriteTo(_stream, CurrentState);
        }

        public TPacket ReadPacket<TPacket>() where TPacket : struct, IResponsePacket
        {
            var packet = default(TPacket);
            var length = _stream.ReadVarInt();
            var id = (byte)_stream.ReadByte();

            packet.Prepare(length, id);
            CurrentState = packet.ReadFrom(_stream, CurrentState);
            return packet;
        }
    }
}
