using UnityEngine;

public class RemotePlayerController : MonoBehaviour
{
    private Vector3 targetPos;
    public float smoothSpeed = 10f;
    public bool snapOnFirstReceive = true;

    private bool hasReceived = false;

    private void Start()
    {
        // register for delivery
        NetworkSimulator.RemotePlayer = this;
        targetPos = transform.position;
    }

    public void ReceiveCompressedData(int xi, int yi, int zi)
    {
        Vector3 decompressed = PositionCompressor.Decompress(xi, yi, zi);

        Debug.Log($"[RECEIVE] Decompressed world pos: {decompressed} | Current world pos: {transform.localPosition}");

        if (!hasReceived && snapOnFirstReceive)
        {
            // immediate placement so initial offsets won't persist
            transform.localPosition = decompressed;
            targetPos = decompressed;
            hasReceived = true;
            Debug.Log("[RECEIVE] Snapped remote to initial world position.");
            return;
        }

        // set target and smooth towards it
        targetPos = decompressed;
        hasReceived = true;
    }

    private void Update()
    {
        if (!hasReceived) return;

        // world-space smoothing
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smoothSpeed);
    }
}
