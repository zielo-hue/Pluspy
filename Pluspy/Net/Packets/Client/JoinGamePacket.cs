﻿using Pluspy.Entities;
using Pluspy.Utilities.Constants;
using System.Net.Sockets;

namespace Pluspy.Net.Packets.Client
{
    public readonly struct JoinGamePacket : IPacket
    {
        private readonly int _playerEntityId;
        private readonly byte _playerGamemode;
        private readonly int _dimension;
        private readonly int _maxPlayersIgnored;
        private readonly string _worldType;
        private readonly int _viewDistance;
        private readonly bool _hideVerboseDebugInfo;

        public JoinGamePacket(
            int playerEntityId, 
            GameMode gamemode,
            Dimension dimension, 
            string worldType, 
            int viewDistance, 
            bool verboseDebugInfo = true)
        {

            _maxPlayersIgnored = 0;
            _playerEntityId = playerEntityId;
            _playerGamemode = (byte)gamemode;
            _dimension = (int)dimension;
            _worldType = worldType;
            _viewDistance = viewDistance;
            _hideVerboseDebugInfo = !verboseDebugInfo;
        }

        public void WriteTo(NetworkStream stream)
        {

        }
    }
}