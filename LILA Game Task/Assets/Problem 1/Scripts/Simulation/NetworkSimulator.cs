public static class NetworkSimulator
{
    public static RemotePlayerController RemotePlayer;

    public static void DeliverToRemote(int x, int y, int z)
    {
        RemotePlayer?.ReceiveCompressedData(x, y, z);
    }
}
