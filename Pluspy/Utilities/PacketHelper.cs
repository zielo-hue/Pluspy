using Pluspy.Attributes;
using Pluspy.Enums;
using Pluspy.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pluspy.Utilities
{
    public static class PacketHelper
    {
        public static Dictionary<Type, Attribute[]> Attributes { get; }
        public static Type[] Types { get; } = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToArray();
        public static Type[][] HandshakePackets { get; }
        public static Type[][] StatusPackets { get; }
        public static Type[][] LoginPackets { get; }
        public static Type[][] PlayPackets { get; }
        public static Type[][][] PacketTypes { get; }

        static PacketHelper()
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

            Attributes = Types
                .Where(x => typeof(IPacket).IsAssignableFrom(x) && x.IsValueType)
                .ToDictionary(x => x, x => x.GetCustomAttributes().ToArray());
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
            return Types
                .Where(type => typeof(IResponsePacket).IsAssignableFrom(type) 
                    && type.IsValueType 
                    && type.GetCustomAttribute<PacketStateAttribute>() is PacketStateAttribute att 
                    && att.State == state)
                .ToList();
        }

        private static List<Type> GetServerPackets(State state)
        {
            return Types
                .Where(type => typeof(IRequestPacket).IsAssignableFrom(type)
                    && type.IsValueType
                    && type.GetCustomAttribute<PacketStateAttribute>() is PacketStateAttribute att
                    && att.State == state)
                .ToList();
        }
    }
}
