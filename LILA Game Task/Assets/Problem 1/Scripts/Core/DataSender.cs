using UnityEngine;

[RequireComponent(typeof(Transform))]
public class DataSender : MonoBehaviour
{
    [Header("Send Settings")]
    public bool sendEveryFrame = true;
    public float sendThreshold = 0.02f;

    private Vector3 lastSentPos;
    private bool hasSentInitial = false;

    private void Start()
    {
        lastSentPos = transform.localPosition;
        // Force an initial sync on start
        SendPosition(transform.position, force: true);
    }

    private void Update()
    {
        if (!sendEveryFrame)
        {
            if (Vector3.Distance(transform.localPosition, lastSentPos) < sendThreshold) return;
        }

        SendPosition(transform.localPosition, force: false);
    }

    public void SendPosition(Vector3 pos, bool force = false)
    {
        if (!force && Vector3.Distance(pos, lastSentPos) < sendThreshold) return;

        var (xi, yi, zi, dataSize) = PositionCompressor.Compress(pos);

        Debug.Log($"[SEND] WorldPos: {pos} | Compressed: ({xi},{yi},{zi}) | DataSize: {dataSize} bits");

        lastSentPos = pos;
        hasSentInitial = true;

        NetworkSimulator.DeliverToRemote(xi, yi, zi);
    }
}
