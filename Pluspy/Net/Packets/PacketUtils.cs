using Pluspy.Attributes;
using Pluspy.Enums;
using Pluspy.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pluspy.Net.Packets
{
    public static class PacketUtils
    {
        private static readonly Type[] _types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToArray();
        public static Type[][] HandshakePackets { get; }
        public static Type[][] StatusPackets { get; }
        public static Type[][] LoginPackets { get; }
        public static Type[][] PlayPackets { get; }
        public static Type[][][] PacketTypes { get; }

        static PacketUtils()
        {
            var clientPackets = GetClientPackets(State.Handshake);
            var serverPackets = GetServerPackets(State.Handshake);

            HandshakePackets = Populate(clientPackets, serverPackets);

            clientPackets = GetClientPackets(State.Status);
            serverPackets = GetServerPackets(State.Status);

            StatusPackets = Populate(clientPackets, serverPackets);

            clientPackets = GetClientPackets(State.Login);
            serverPackets = GetServerPackets(State.Login);

            LoginPackets = Populate(clientPackets, serverPackets);

            clientPackets = GetClientPackets(State.Play);
            serverPackets = GetServerPackets(State.Play);

            PlayPackets = Populate(clientPackets, serverPackets);

            PacketTypes = new Type[][][]
            {
                HandshakePackets,
                StatusPackets,
                LoginPackets,
                PlayPackets
            };
        }

        private static Type[][] Populate(List<Type> clientPackets, List<Type> serverPackets)
        {
            var array = new Type[Math.Max(clientPackets.Count, serverPackets.Count)][];

            for (var i = 0; i < array.Length; i++)
                array[i] = new[] { i < clientPackets.Count ? clientPackets[i] : null, i < serverPackets.Count ? serverPackets[i] : null };
            
            clientPackets.Clear();
            serverPackets.Clear();
            return array;
        }

        private static List<Type> GetClientPackets(State state)
        {
            return _types
                .Where(type => typeof(IPacket).IsAssignableFrom(type) 
                    && type.IsValueType 
                    && type.GetCustomAttribute<Clientbound>() is Clientbound att 
                    && att.State == state)
                .ToList();
        }

        private static List<Type> GetServerPackets(State state)
        {
            return _types
                .Where(type => typeof(IPacket).IsAssignableFrom(type)
                    && type.IsValueType
                    && type.GetCustomAttribute<Serverbound>() is Serverbound att
                    && att.State == state)
                .ToList();
        }
    }
}
