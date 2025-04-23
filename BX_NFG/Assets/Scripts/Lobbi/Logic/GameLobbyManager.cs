
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Assets.Scripts.Lobbi;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Datas;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Lobbi.Players;
using Assets.Scripts.UI.LobbyUI;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Connection.Lobbi
{
    public class GameLobbyManager : Singleton<GameLobbyManager>
    {
        private List<LobbyPlayerData> playersData = new List<LobbyPlayerData>();
        private LobbyPlayerData localPlayerData;
        // private LobbyData lobbyData;
        bool joined = false;

        private int maxPlayers = 10;
        private void OnEnable()
        {
            LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        }

        public async Task<bool> CreateLobby()
        {
            LobbyPlayerData playerData = new LobbyPlayerData();
            LobbyData lobbyData = new LobbyData();
            playerData.Inizialize(AuthenticationService.Instance.PlayerId, "HostPlayer");
            bool success = await LobbyManager.Instance.CreateLobby(maxPlayers, false, playerData.Serialize(), lobbyData.Serialize());
            return success;
        }

        public async Task<bool> JoinLobby(string code)
        {
            LobbyPlayerData playerData = new LobbyPlayerData();
            playerData.Inizialize(AuthenticationService.Instance.PlayerId, "JoinPlayer");
            bool success = await LobbyManager.Instance.JoinLobby(code, playerData.Serialize());
            return success;
        }

        //-------------EVENTS----------------

        private async void OnLobbyUpdated(Lobby lobby)
        {
            List<Dictionary<string, PlayerDataObject>> players = LobbyManager.Instance.GetPlayersData();
            playersData.Clear();

            int numberOfPlayersReady = 0;

            players.ForEach(playerData => 
            {
                GenerateData(playerData);
                numberOfPlayersReady = NumberOfPlayersReady(playerData, numberOfPlayersReady);
            });


            GameLobbyEvents.OnLobbyUpdated?.Invoke();

            if (numberOfPlayersReady == players.Count)
            {
                GameLobbyEvents.OnLobbyReady?.Invoke();
            }

            else
            {
                GameLobbyEvents.OnLobbyCancel?.Invoke();
            }

            if (LobbyManager.Instance.GetRelayCode() != null && !joined)
            {
                await JoinRelayServer();
                await SceneManager.LoadSceneAsync("GameScene");
                joined = true;
            }
        }

        //-----------------------------------------

        private void GenerateData(Dictionary<string, PlayerDataObject> data)
        {
            LobbyPlayerData playerData = new LobbyPlayerData();
            playerData.Inizialice(data);

            if (AuthenticationService.Instance.PlayerId == playerData.Id)
            {
                localPlayerData = playerData;
            }

            playersData.Add(playerData);
        }

        private int NumberOfPlayersReady(Dictionary<string, PlayerDataObject> data, int playersReady)
        {
            LobbyPlayerData playerData = new LobbyPlayerData();
            playerData.Inizialice(data);

            if(playerData.IsReady)
            {
                return playersReady + 1;
            }

            return playersReady;
        }

        //------------------ GETTTERS ------------------//
        public string GetLobbyCode()
        {
            return LobbyManager.Instance.GetLobbyCode();
        }

        public List<LobbyPlayerData> GetPlayerDataList()
        {
            return playersData;
        }

        public string GetLocalID()
        {
            return localPlayerData.Id;
        }


        //------------------ SETTTERS ------------------//
        public async Task<bool> SetPlayerReady()
        {
            localPlayerData.IsReady = true;
            return await LobbyManager.Instance.UpdatePlayerData(localPlayerData.Id, localPlayerData.Serialize());
        }

        public async Task<bool> SetPlayerNotReady()
        {
            localPlayerData.IsReady = false;
            return await LobbyManager.Instance.UpdatePlayerData(localPlayerData.Id, localPlayerData.Serialize());
        }

        public async Task<bool> SetPlayerTeam(LobbyPlayerData playerData, PlayerTeam playerTeam)
        {
            playerData.PlayerTeam = playerTeam;
            return await LobbyManager.Instance.UpdatePlayerData(playerData.Id, playerData.Serialize());
        }

        //-----------------------------------------------

        public async Task StartRelayServer()
        {
            string relayCode = await RelayManager.Instance.CreateRelay(maxPlayers);
            LobbyData lobbyData = new LobbyData();
            lobbyData.Inizialice(relayCode, "GameScene");
            await LobbyManager.Instance.UpdateLobbyData(lobbyData.Serialize());

            string allocationId = RelayManager.Instance.GetAllocatorId();
            string connectionData = RelayManager.Instance.GetConnectionData();

            await LobbyManager.Instance.UpdatePlayerData(localPlayerData.Id, localPlayerData.Serialize(), allocationId, connectionData);

            await SceneManager.LoadSceneAsync("GameScene");
        }

        private async Task<bool> JoinRelayServer()
        {
            await RelayManager.Instance.JoinRelay(LobbyManager.Instance.GetRelayCode());

            string allocationId = RelayManager.Instance.GetAllocatorId();
            string connectionData = RelayManager.Instance.GetConnectionData();

            await Task.Delay(200);
            await LobbyManager.Instance.UpdatePlayerData(localPlayerData.Id, localPlayerData.Serialize(), allocationId, connectionData);

            return true;
        }
    }
}