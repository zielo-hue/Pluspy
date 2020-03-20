using Pluspy.Entities;
using Pluspy.Enums;
using Pluspy.Utilities;
using System;
using System.Net.Sockets;
using System.Text;

namespace Pluspy.Net.Packets.Client
{
    public struct ServerListPingResponsePacket : IPacket
    {
        public string SerializedData { get; private set; }

        public ServerListPingResponsePacket(ServerListPingResponseModel model)
        {
            SerializedData = model.ToString();
        }

        public State ReadFrom(NetworkStream stream, State state, PacketType type)
        {
            SerializedData = stream.ReadString();
            return state;
        }

        public readonly State WriteTo(NetworkStream stream, State state, PacketType type)
        {
            var serializedDataByteCount = Encoding.UTF8.GetByteCount(SerializedData);
            Span<byte> dataVarIntPrefixBytes = stackalloc byte[5];
            var dataVarIntLength = VarIntUtilities.GetBytes(serializedDataByteCount, dataVarIntPrefixBytes);
            Span<byte> lengthBytesSpan = stackalloc byte[5];
            var lengthBytesLength = VarIntUtilities.GetBytes(dataVarIntLength + serializedDataByteCount + 1, lengthBytesSpan);
            var lengthBytes = lengthBytesSpan[..lengthBytesLength];
            Span<byte> data = stackalloc byte[dataVarIntLength + lengthBytes.Length + serializedDataByteCount + 1];

            lengthBytes.CopyTo(data);
            data[lengthBytes.Length] = (int)ServerPacket.ServerListPingResponse;
            dataVarIntPrefixBytes[..dataVarIntLength].CopyTo(data[(lengthBytes.Length + 1)..]);
            Encoding.UTF8.GetBytes(SerializedData, data[(dataVarIntLength + lengthBytes.Length + 1)..]);
            stream.Write(data);

            return state;
        }
    }
}
