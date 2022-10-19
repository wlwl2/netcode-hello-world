using Unity.Netcode;
using UnityEngine;

public class NetworkVariableTest : NetworkBehaviour {
    private NetworkVariable<float> ServerNetworkVariable = new();
    private float last_t = 0.0f;

    public override void OnNetworkSpawn () {
        if (IsServer) {
            ServerNetworkVariable.Value = 0.0f;
            Debug.Log("Server's var initialized to: " + ServerNetworkVariable.Value);
        }
    }

    private void Update () {
        float t_now = Time.time;
        if (IsServer) {
            ServerNetworkVariable.Value += 0.1f;
            if (t_now - last_t > 0.5f) {
                last_t = t_now;
                Debug.Log("Server set its var to: " + ServerNetworkVariable.Value);
            }
        }
    }
}