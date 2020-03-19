using Pluspy.Entities;
using Pluspy.Constants;
using System;
using System.Text;
using Pluspy.Utilities;
using Pluspy.Core;
using Pluspy.Enums;

namespace Pluspy.Net.Packets.Client
{
    public struct ServerListPingResponsePacket : IPacket
    {
        public string SerializedData { get; private set; }

        public ServerListPingResponsePacket(ServerListPingResponseModel model)
        {
            SerializedData = model.ToString();
        }

        public State ReadFrom(MinecraftNetworkStream stream, State state, PacketType type)
        {
            SerializedData = stream.ReadString();
            return state;
        }

        public readonly State WriteTo(MinecraftNetworkStream stream, State state, PacketType type)
        {
            var serializedDataByteCount = Encoding.UTF8.GetByteCount(SerializedData);
            Span<byte> dataVarIntPrefixBytes = stackalloc byte[5];

            serializedDataByteCount.GetVarIntBytes(dataVarIntPrefixBytes, out int dataVarIntLength);

            Span<byte> lengthBytesSpan = stackalloc byte[5];

            (dataVarIntLength + serializedDataByteCount + 1).GetVarIntBytes(lengthBytesSpan, out int lengthBytesLength);

            var lengthBytes = lengthBytesSpan[..lengthBytesLength];

            Span<byte> data = stackalloc byte[dataVarIntLength + lengthBytes.Length + serializedDataByteCount + 1];

            lengthBytes.CopyTo(data);

            data[lengthBytes.Length] = (int)ServerPacket.ServerListPingResponse;

            dataVarIntPrefixBytes[..dataVarIntLength].CopyTo(data[(lengthBytes.Length + 1)..]);
            Encoding.UTF8.GetBytes(SerializedData, data.Slice(dataVarIntLength + lengthBytes.Length + 1));
            stream.Write(data);
            return state;
        }
    }
}
