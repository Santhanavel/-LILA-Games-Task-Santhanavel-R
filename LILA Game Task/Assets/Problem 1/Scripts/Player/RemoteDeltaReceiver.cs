using UnityEngine;


public class RemoteDeltaReceiver : MonoBehaviour
{
    private Vector3 pendingDelta;
    public float speed = 5f;
    public void ApplyDelta(Vector3 delta)
    {
        pendingDelta = delta;
    }

    void Update()
    {
        if (pendingDelta != Vector3.zero)
        {
             transform.position += pendingDelta;
        }
        pendingDelta = Vector3.zero;
    }
}