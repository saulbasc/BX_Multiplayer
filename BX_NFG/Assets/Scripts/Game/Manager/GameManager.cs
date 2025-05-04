
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Assets.Scripts.Game.Manager
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
            if (LobbyPlayersManager.Instance.IsHost())
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
            NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionApproval;
            (byte[] allocationId, byte[] key, byte[] connectionData, string ip, int port) = HostRelayManager.Instance.GetHostConnectionData();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(ip, (ushort)port, allocationId, key, connectionData, true);
            NetworkManager.Singleton.StartHost();
        }

        private void ClientConnection()
        {
            (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string ip, int port) = ClientRelayManager.Instance.GetClientConnectionData();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(ip, (ushort)port, allocationId, key, connectionData, hostConnectionData, true);
            NetworkManager.Singleton.StartClient();
        }

        private void ConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            response.Approved = true;
            response.CreatePlayerObject = true;
            response.Pending = false;
        }
    }
}
