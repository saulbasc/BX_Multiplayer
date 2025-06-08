using System.Collections;
using Assets.Scripts.Game.GameEvents.Player;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Lobbi.Players;
using Assets.Scripts.Relay;
using Assets.Scripts.Sound;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace Assets.Scripts.Game.Manager
{
    public class GameEntryManager : NetworkBehaviour
    {
        private HostRelayManager hostRelayManager;
        private ClientRelayManager clientRelayManager;
        private LobbyDataManager lobbyDataManager;
        private LobbyPlayerManager lobbyPlayerManager;

        [SerializeField] private MatchInfo matchInfo;
        [SerializeField] private GameObject playerPanel;
        [SerializeField] private GameObject spectatorPanel;

        private void Awake()
        {
            SoundEvents.Instance.RaiseEndGameSound();
        }

        public override void OnNetworkSpawn()
        {
            NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
            StartCoroutine(WaitForLobbyDataManagerAndInit());
        }

        private IEnumerator WaitForLobbyDataManagerAndInit()
        {
            while (lobbyDataManager == null || lobbyPlayerManager == null || clientRelayManager == null || hostRelayManager == null)
            {
                lobbyDataManager = FindFirstObjectByType<LobbyDataManager>();
                if (lobbyDataManager == null)
                {
                    yield return null; 
                }

                lobbyPlayerManager = FindFirstObjectByType<LobbyPlayerManager>();
                if (lobbyPlayerManager == null)
                {
                    yield return null;
                }

                clientRelayManager = FindFirstObjectByType<ClientRelayManager>();
                if (clientRelayManager == null)
                {
                    yield return null;
                }

                hostRelayManager = FindFirstObjectByType<HostRelayManager>();
                if (hostRelayManager == null)
                {
                    yield return null;
                }
            }

            if (lobbyDataManager.IsLocalPlayerHost())
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
            (byte[] allocationId, byte[] key, byte[] connectionData, string ip, int port) = hostRelayManager.GetHostConnectionData();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(ip, (ushort)port, allocationId, key, connectionData, true);
            LobbyPlayerData lpd = lobbyPlayerManager.GetSinglePlayerDataObject(UnityServicesActions.GetCurrentUserID());
            if (lpd.PlayerTeam == PlayerTeam.Spectator)
            {
                ulong localClientId = NetworkManager.Singleton.LocalClientId;
                var playerObject = NetworkManager.Singleton.ConnectedClients[localClientId].PlayerObject;
                var playerInGame = playerObject.GetComponent<PlayerInGame>();
                playerInGame.SetSpectator();
                playerPanel.SetActive(false);
                spectatorPanel.SetActive(true);
                return;
            }
            playerPanel.SetActive(true);
            spectatorPanel.SetActive(false);
        }

        private void SetMatchInfo()
        {
            int numberOfLocalPlayers = lobbyDataManager.GetNumberOfPlayersInLobbyTeams(PlayerTeam.Local);
            int numberOfVisitorPlayers = lobbyDataManager.GetNumberOfPlayersInLobbyTeams(PlayerTeam.Visitor);
            matchInfo.SetNumberOfPlayersInTeams(numberOfLocalPlayers + numberOfVisitorPlayers);
            matchInfo.SetMatchDuration(lobbyDataManager.GetLobbyMatchDuration());
        }

        private void ClientConnection()
        {
            (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string ip, int port) = clientRelayManager.GetClientConnectionData();
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(ip, (ushort)port, allocationId, key, connectionData, hostConnectionData, true);
            ulong localClientId = NetworkManager.Singleton.LocalClientId;
            LobbyPlayerData lpd = lobbyPlayerManager.GetSinglePlayerDataObject(UnityServicesActions.GetCurrentUserID());
            RegisterPlayerConnectionServerRpc(OwnerClientId, localClientId, lpd.PlayerTeam);
        }

        [ServerRpc(RequireOwnership = false)]
        public void RegisterPlayerConnectionServerRpc(ulong playerGameId, ulong clientId, PlayerTeam playerTeam)
        {
            if (playerTeam == PlayerTeam.Spectator)
            {
                var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
                var playerInGame = playerObject.GetComponent<PlayerInGame>();
                playerInGame.SetSpectator();
                playerPanel.SetActive(false);
                spectatorPanel.SetActive(true);
                return;
            }
        }
    }
}
