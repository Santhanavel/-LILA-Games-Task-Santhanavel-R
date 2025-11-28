using UnityEngine;

public class SimpleNetworkSimulator : MonoBehaviour
{
    public static RemoteDeltaReceiver receiver;

    public static void SendCompressed(int xi, int yi, int zi)
    {
        if (receiver == null) return;

        Vector3 decompressed = PositionDeltaCompressor.Decompress(xi, yi, zi);

        receiver.ApplyDelta(decompressed);
    }
}
