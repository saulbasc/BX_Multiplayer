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
using UnityEngine;
using Assets.Scripts.Lobbi.Data;

namespace Assets.Scripts.Relay
{
    public class HostRelayManager : Singleton<HostRelayManager>
    {
        private int maxConnections = 10;
        private HostRelayData hostRelayData;
        public string GetAllocatorId() => hostRelayData.AllocationId.ToString();
        public string GetConnectionData() => Convert.ToBase64String(hostRelayData.ConnectionData);


        public async Task<bool> StartRelayServer()
        {
            return await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);

                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.FirstOrDefault(connection => connection.ConnectionType == "dtls");

                if (dtlsEndpoint == null)
                {
                    throw new Exception("No se encontró endpoint DTLS");
                }

                hostRelayData = new HostRelayData(
                    dtlsEndpoint.Host, dtlsEndpoint.Port, allocation.ConnectionData,
                    allocation.Key, allocation.AllocationIdBytes, allocation.AllocationId
                );

                PlayerStatus.Instance.InGame = true;
                await UpdateLobbyData(joinCode);
                await UpdateLobbyPlayerData();

                NetworkManager.Singleton.StartHost();
                LoadGameScene();

                return true;

            }, false);
        }

        private async Task<bool> UpdateLobbyData(string joinCode)
        {
            SetTotalPlayersInTeamsInMatchInfo();
            MatchInfo.Instance.SetMatchDuration(LobbyDataManager.Instance.GetLobbyMatchDuration());
            LobbyData actualLobbyData = LobbyDataManager.Instance.GetLobbyDataObject();
            var lobbyData = new LobbyData(joinCode, actualLobbyData.MatchDuration);
            return await LobbyDataManager.Instance.UpdateLobbyData(lobbyData.SerializeObjectToDictionary());
        }

        /// <summary>
        /// Establece el número total de jugadores en Local y Visitante en MatchInfo.
        /// </summary>
        private void SetTotalPlayersInTeamsInMatchInfo()
        {
            int numberOfLocalPlayers = LobbyDataManager.Instance.GetNumberOfPlayersInLobbyTeams(PlayerTeam.Local);
            int numberOfVisitorPlayers = LobbyDataManager.Instance.GetNumberOfPlayersInLobbyTeams(PlayerTeam.Visitor);
            MatchInfo.Instance.SetNumberOfPlayersInTeams(numberOfLocalPlayers + numberOfVisitorPlayers);
        }

        private async Task<bool> UpdateLobbyPlayerData()
        {
            return await LobbyPlayerManager.Instance.UpdatePlayerOptions(UnityServicesActions.GetCurrentUserID(), GetAllocatorId(), GetConnectionData());
        }

        private void LoadGameScene()
        {
            NetworkManager.Singleton.SceneManager.LoadScene(
                Scenes.GameScene.ToString(),
                LoadSceneMode.Single
            );
        }

        public (byte[] allocationId, byte[] key, byte[] connectionData, string ip, int port) GetHostConnectionData()
        {
            return hostRelayData.GetConnectionData();
        }
    }
}
