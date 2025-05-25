using Assets.Scripts.Game.GameEvents.Player;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace Assets.Scripts.Game.Manager
{
    public class GameEntryManager : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
            if (LobbyDataManager.Instance.IsLocalPlayerHost())
            {
                HostConnection();
            }
            else
            {
                ClientConnection();
            }
        }

        private void HostConnection()
        {
            (byte[] allocationId, byte[] key, byte[] connectionData, string ip, int port) = HostRelayManager.Instance.GetHostConnectionData();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(ip, (ushort)port, allocationId, key, connectionData, true);
            ulong localClientId = NetworkManager.Singleton.LocalClientId;
            var playerObject = NetworkManager.Singleton.ConnectedClients[localClientId].PlayerObject;
            var playerInGame = playerObject.GetComponent<PlayerInGame>();
            PlayerConnectionMap.Instance.RegisterPlayer(localClientId, playerInGame);
        }

        private void ClientConnection()
        {
            (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string ip, int port) = ClientRelayManager.Instance.GetClientConnectionData();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(ip, (ushort)port, allocationId, key, connectionData, hostConnectionData, true);
            ulong localClientId = NetworkManager.Singleton.LocalClientId;
            RegisterPlayerConnectionServerRpc(localClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        public void RegisterPlayerConnectionServerRpc(ulong clientId)
        {
            var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            var playerInGame = playerObject.GetComponent<PlayerInGame>();
            PlayerConnectionMap.Instance.RegisterPlayer(clientId, playerInGame);
        }
    }
}
