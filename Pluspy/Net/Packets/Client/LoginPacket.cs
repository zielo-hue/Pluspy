using Pluspy.Entities;
using Pluspy.Utilities;
using Pluspy.Utilities.Constants;
using System;
using System.Net.Sockets;
using System.Text;

namespace Pluspy.Net.Packets.Client
{
    public readonly struct LoginPacket : IPacket
    {
        private readonly string _username;
        private readonly string _uuid;

        public LoginPacket(string username, string uuid)
        {
            _username = username;
            _uuid = uuid;
        }

        public LoginPacket(UserModel model)
        {
            _username = model.Username;
            _uuid = model.UUID;
        }

        public void WriteTo(NetworkStream stream)
        {
            var usernameByteCount = Encoding.UTF8.GetByteCount(_username);
            var uuidByteCount = Encoding.UTF8.GetByteCount(_uuid);

            Span<byte> uuidLengthBytes = stackalloc byte[5];

            uuidByteCount.GetVarIntBytes(uuidLengthBytes, out int uuidLengthBytesLength);

            Span<byte> usernameLengthBytes = stackalloc byte[5];

            usernameByteCount.GetVarIntBytes(usernameLengthBytes, out int usernameLengthBytesLength);

            var dataLength = uuidLengthBytesLength + usernameLengthBytesLength + Encoding.UTF8.GetByteCount(_uuid) + Encoding.UTF8.GetByteCount(_username) + 1;

            Span<byte> packetLengthBytes = stackalloc byte[5];

            (uuidLengthBytesLength + usernameLengthBytesLength + 1).GetVarIntBytes(packetLengthBytes, out int packetLengthBytesLength);

            Span<byte> data = stackalloc byte[dataLength + packetLengthBytesLength];

            packetLengthBytes[..packetLengthBytesLength].CopyTo(data);
            data[packetLengthBytesLength + 1] = (byte)ServerPacket.LoginSuccess;
            uuidLengthBytes[..uuidLengthBytesLength].CopyTo(data[(packetLengthBytesLength + 1)..]);
            Encoding.UTF8.GetBytes(_uuid, data[(packetLengthBytesLength + uuidLengthBytesLength + 1)..]);
            usernameLengthBytes[..usernameLengthBytesLength].CopyTo(data[(packetLengthBytesLength + uuidLengthBytesLength + uuidByteCount + 1)..]);
            Encoding.UTF8.GetBytes(_username, data[(packetLengthBytesLength + uuidLengthBytesLength + uuidByteCount + usernameLengthBytesLength + 1)..]);
            stream.Write(data);
        }
    }
}
