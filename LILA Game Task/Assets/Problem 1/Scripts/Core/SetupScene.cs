using UnityEngine;

public class SetupScene : MonoBehaviour
{
    public RemoteDeltaReceiver remotePlayer;

    void Start()
    {
        SimpleNetworkSimulator.receiver = remotePlayer;
    }
}
