using Unity.Netcode;
using UnityEngine;

namespace HelloWorld {
    public class HelloWorldPlayer : NetworkBehaviour {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

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
                Vector3 randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            } else {
                SubmitPositionRequestServerRpc();
            }
        }

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