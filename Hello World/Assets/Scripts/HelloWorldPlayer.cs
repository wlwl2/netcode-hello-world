using Unity.Netcode;
using UnityEngine;

namespace HelloWorld {
    public class HelloWorldPlayer : NetworkBehaviour {
        /// <summary>
        ///  NetworkVariable to represent this player's networked position.
        /// </summary>
        public NetworkVariable<Vector3> Position = new();

        /// <summary>
        ///  OnNetworkSpawn() can be overriden on any NetworkBehaviour. We
        ///  override OnNetworkSpawn since a client and a server will run
        ///  different logic here. 
        /// </summary>
        public override void OnNetworkSpawn () {
            // On both client and server instances of this player, we call the
            // Move() method
            if (IsOwner) Move();
        }

        public void Move () {
            if (NetworkManager.Singleton.IsServer) {
                // If this player is a server-owned player, at OnNetworkSpawn()
                // we can immediately move this player, as suggested in the
                // following code.
                Vector3 randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }

            if (NetworkManager.Singleton.IsClient) {
                // If we are a client, we call a ServerRpc. A ServerRpc can be
                // invoked by a client to be executed on the server.
                SubmitPositionRequestServerRpc();
            }
        }

        /// <summary>
        ///  This ServerRpc simply sets the position NetworkVariable on the
        ///  server's instance of this player by just picking a random point on
        ///  the plane. If we are a client, we call a ServerRpc.
        ///  This is later transformed on both the server and client in the
        ///  Update() loop.
        /// </summary>
        [ServerRpc]
        private void SubmitPositionRequestServerRpc (ServerRpcParams rpcParams = default) {
            Position.Value = GetRandomPositionOnPlane();
        }

        private static Vector3 GetRandomPositionOnPlane () {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        private void Update () {
            transform.position = Position.Value;
        }
    }
}