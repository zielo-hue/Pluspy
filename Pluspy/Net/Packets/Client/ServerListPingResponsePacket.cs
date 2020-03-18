using Pluspy.Entities;
using Pluspy.Constants;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Pluspy.Net.Packets.Client
{
    public readonly struct ServerListPingResponsePacket : IPacket
    {
        private readonly string _serializedData;

        public ServerListPingResponsePacket(
            string serverVersion, 
            int protocolVersion, 
            int currentPlayers, 
            int maximumPlayers,
            List<UserModel> onlinePlayerSample, 
            Text serverDescription, 
            string? faviconString = null)
        {
            _serializedData = JsonSerializer.Serialize(
                new ServerListPingResponse(
                        new ServerListPingResponseVersion(serverVersion, protocolVersion),
                        new ServerListPingResponsePlayerList(maximumPlayers, currentPlayers, onlinePlayerSample),
                        serverDescription,
                        faviconString), 
                new JsonSerializerOptions 
                { 
                    IgnoreNullValues = true 
                });
        }

        public void WriteTo(NetworkStream stream)
        {
            var serializedDataByteCount = Encoding.UTF8.GetByteCount(_serializedData);
            Span<byte> dataVarIntPrefixBytes = stackalloc byte[5];

            serializedDataByteCount.GetVarIntBytes(dataVarIntPrefixBytes, out int dataVarIntLength);

            Span<byte> lengthBytesSpan = stackalloc byte[5];

            (dataVarIntLength + serializedDataByteCount + 1).GetVarIntBytes(lengthBytesSpan, out int lengthBytesLength);

            var lengthBytes = lengthBytesSpan[..lengthBytesLength];

            Span<byte> data = stackalloc byte[dataVarIntLength + lengthBytes.Length + serializedDataByteCount + 1];

            lengthBytes.CopyTo(data);

            data[lengthBytes.Length] = (int)ServerPacket.ServerListPingResponse;

            dataVarIntPrefixBytes[..dataVarIntLength].CopyTo(data[(lengthBytes.Length + 1)..]);
            Encoding.UTF8.GetBytes(_serializedData, data.Slice(dataVarIntLength + lengthBytes.Length + 1));
            stream.Write(data);
        }
    }
}
