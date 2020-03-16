namespace Pluspy.Utilities.Constants
{
    public enum ServerPacket
    {
        ServerListPingResponse = 0x00,
        ServerListPingPong = 0x01,
        LoginSuccess = 0x02,
        JoinGame = 0x25,
        PlayerPositionAndLook = 0x32,
        SpawnPosition = 0x49
    }
}
