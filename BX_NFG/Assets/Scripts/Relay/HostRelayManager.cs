using Assets.Scripts.Commons;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Datas;
using Assets.Scripts.Lobbi.Logic;
using System;
using System.Threading.Tasks;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine.SceneManagement;
using System.Linq;
using Assets.Scripts.Handlers;
using Unity.Netcode;
using Assets.Scripts.Lobbi.Data;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Assets.Scripts.Lobbi.Players;
using System.Collections;

namespace Assets.Scripts.Relay
{
    public class HostRelayManager : Singleton<HostRelayManager>
    {
        private int maxConnections = 10;
        private HostRelayData hostRelayData;
        private string playerType;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionApproval;
        }

        public async Task<bool> StartRelayServer()
        {
            return await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                PlayerStatus.Instance.InGame = true;
                string code = await InitializeHostRelayData();
                await UpdateLobbyData(code);
                await UpdateLobbyPlayerData();

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                    hostRelayData.Ip,
                    (ushort)hostRelayData.Port,
                    hostRelayData.AllocationIdBytes,
                    hostRelayData.Key,
                    hostRelayData.ConnectionData
                );

                NetworkManager.Singleton.StartHost();
                LoadGameScene();
                return true;

            }, false);
        }

        private async Task<string> InitializeHostRelayData()
        {
            return await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.FirstOrDefault(connection => connection.ConnectionType == "udp");

                hostRelayData = new HostRelayData(
                    dtlsEndpoint.Host, dtlsEndpoint.Port, allocation.ConnectionData,
                    allocation.Key, allocation.AllocationIdBytes, allocation.AllocationId
                );

                Debug.Log($"Relay Allocation Created: {allocation.AllocationId}");
                Debug.Log($"Join Code: {joinCode}");
                Debug.Log($"Relay Endpoint: {dtlsEndpoint.Host}:{dtlsEndpoint.Port}");


                return joinCode;
            }, "");
        }

        private async Task<bool> UpdateLobbyData(string joinCode)
        {
            SetMatchInfo();
            LobbyData actualLobbyData = LobbyDataManager.Instance.GetLobbyDataObject();
            var lobbyData = new LobbyData(joinCode, actualLobbyData.MatchDuration);
            return await LobbyDataManager.Instance.UpdateLobbyData(lobbyData.SerializeObjectToDictionary());
        }

        /// <summary>
        /// Establece el número total de jugadores en Local y Visitante en MatchInfo.
        /// </summary>
        private void SetMatchInfo()
        {
            int numberOfLocalPlayers = LobbyDataManager.Instance.GetNumberOfPlayersInLobbyTeams(PlayerTeam.Local);
            int numberOfVisitorPlayers = LobbyDataManager.Instance.GetNumberOfPlayersInLobbyTeams(PlayerTeam.Visitor);
            MatchInfo.Instance.SetNumberOfPlayersInTeams(numberOfLocalPlayers + numberOfVisitorPlayers);
            MatchInfo.Instance.SetMatchDuration(LobbyDataManager.Instance.GetLobbyMatchDuration());
        }

        private async Task<bool> UpdateLobbyPlayerData()
        {
            return await LobbyPlayerManager.Instance.UpdatePlayerOptions(
                UnityServicesActions.GetCurrentUserID(), 
                hostRelayData.AllocationId.ToString(),
                Convert.ToBase64String(hostRelayData.ConnectionData)
            );
        }

        private void LoadGameScene()
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }

        public (byte[] allocationId, byte[] key, byte[] connectionData, string ip, int port) GetHostConnectionData()
        {
            return hostRelayData.GetConnectionData();
        }

        private void ConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            string payload = System.Text.Encoding.UTF8.GetString(request.Payload);
            response.Approved = true;
            response.CreatePlayerObject = true;
            response.Pending = false;
        }
    }
}
