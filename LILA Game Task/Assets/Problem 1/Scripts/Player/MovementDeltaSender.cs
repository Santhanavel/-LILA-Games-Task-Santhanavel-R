using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MovementDeltaSender : MonoBehaviour
{
    Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPos = transform.position;

        Vector3 movementDelta = currentPos - lastPosition;

        // Send delta only if moved
        if (movementDelta.sqrMagnitude > 0.00001f)
        {
            SendCompressedDelta(movementDelta);
        }
        else
        {
            // send zero delta so remote stops
            SendCompressedDelta(Vector3.zero);
        }

        lastPosition = currentPos;
    }

    // --------------------------
    // NEW: compressed delta + bit debug
    // --------------------------
    void SendCompressedDelta(Vector3 delta)
    {
        var (xi, yi, zi, bitsPerAxis) = PositionDeltaCompressor.Compress(delta);
        Debug.Log($"[SEND] WorldPos: {delta} | Compressed: ({xi},{yi},{zi}) | DataSize: {bitsPerAxis} bits");
        // Deliver compressed ints to network
        SimpleNetworkSimulator.SendCompressed(xi, yi, zi);
    }
}
