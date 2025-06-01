using System.Diagnostics;
using Assets.Scripts.Game.GameEvents.Player;
using Assets.Scripts.Lobbi.Data;
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
            if(playerInGame.Team == PlayerTeam.Local)
            {
                MatchInfo.Instance.Match.LocalTeam.AddNewPlayer(OwnerClientId, playerInGame.PlayerId);
            }else if(playerInGame.Team == PlayerTeam.Visitor)
            {
                MatchInfo.Instance.Match.VisitorTeam.AddNewPlayer(OwnerClientId, playerInGame.PlayerId);
            }
        }

        private void ClientConnection()
        {
            (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string ip, int port) = ClientRelayManager.Instance.GetClientConnectionData();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(ip, (ushort)port, allocationId, key, connectionData, hostConnectionData, true);
            ulong localClientId = NetworkManager.Singleton.LocalClientId;
            RegisterPlayerConnectionServerRpc(OwnerClientId, localClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        public void RegisterPlayerConnectionServerRpc(ulong playerGameId, ulong clientId)
        {
            var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            var playerInGame = playerObject.GetComponent<PlayerInGame>();
            PlayerConnectionMap.Instance.RegisterPlayer(clientId, playerInGame);
            if (playerInGame.Team == PlayerTeam.Local)
            {
                MatchInfo.Instance.Match.LocalTeam.AddNewPlayer(playerGameId, playerInGame.PlayerId);
            }
            else if (playerInGame.Team == PlayerTeam.Visitor)
            {
                MatchInfo.Instance.Match.VisitorTeam.AddNewPlayer(playerGameId, playerInGame.PlayerId);
            }
        }
    }
}
