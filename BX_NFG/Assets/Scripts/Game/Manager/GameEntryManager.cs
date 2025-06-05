using Assets.Scripts.Game.GameEvents.Player;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Lobbi.Players;
using Assets.Scripts.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Assets.Scripts.Game.Manager
{
    public class GameEntryManager : NetworkBehaviour
    {
        [SerializeField] private MatchInfo matchInfo;
        [SerializeField] private GameObject playerPanel;
        [SerializeField] private GameObject spectatorPanel;

        public override void OnNetworkSpawn()
        {
            Debug.Log("GameEntryManager OnNetworkSpawn called");
            NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
            if (LobbyDataManager.Instance.IsLocalPlayerHost())
            {
                HostConnection();
                SetMatchInfo();
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
            LobbyPlayerData lpd = LobbyPlayerManager.Instance.GetSinglePlayerDataObject(UnityServicesActions.GetCurrentUserID());
            Debug.Log("HOST TEAM => " + lpd.PlayerTeam);
            if (lpd.PlayerTeam == PlayerTeam.Spectator)
            {
                playerPanel.SetActive(false);
                spectatorPanel.SetActive(true);
                return;
            }
            ulong localClientId = NetworkManager.Singleton.LocalClientId;
            var playerObject = NetworkManager.Singleton.ConnectedClients[localClientId].PlayerObject;
            var playerInGame = playerObject.GetComponent<PlayerInGame>();
            playerPanel.SetActive(true);
            spectatorPanel.SetActive(false);
            Debug.Log("OOOOOOOOOOOOOOOOOOOOOOOOOOOOHOST");
        }

        private void SetMatchInfo()
        {
            int numberOfLocalPlayers = LobbyDataManager.Instance.GetNumberOfPlayersInLobbyTeams(PlayerTeam.Local);
            int numberOfVisitorPlayers = LobbyDataManager.Instance.GetNumberOfPlayersInLobbyTeams(PlayerTeam.Visitor);
            matchInfo.SetNumberOfPlayersInTeams(numberOfLocalPlayers + numberOfVisitorPlayers);
            matchInfo.SetMatchDuration(LobbyDataManager.Instance.GetLobbyMatchDuration());
        }

        private void ClientConnection()
        {
            (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string ip, int port) = ClientRelayManager.Instance.GetClientConnectionData();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(ip, (ushort)port, allocationId, key, connectionData, hostConnectionData, true);
            ulong localClientId = NetworkManager.Singleton.LocalClientId;
            RegisterPlayerConnectionServerRpc(OwnerClientId, localClientId);
            Debug.Log("OOOOOOOOOOOOOOOOOOOOOOOOOOOOCLIENT");
        }

        [ServerRpc(RequireOwnership = false)]
        public void RegisterPlayerConnectionServerRpc(ulong playerGameId, ulong clientId)
        {
            var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            var playerInGame = playerObject.GetComponent<PlayerInGame>();
        }
    }
}
