using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Assets.Scripts.Lobbi;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Datas;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Lobbi.Players;
using Assets.Scripts.Lobbi.Util;
using Assets.Scripts.UI.LobbyUI;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Connection.Lobbi
{
    public class GameLobbyManager : Singleton<GameLobbyManager>
    {
        bool joined = false;
        bool inGame = false;

        private int maxPlayers = 10;

        private void OnEnable()
        {
            LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
            Destroy(gameObject);
        }

        public async Task<bool> CreateLobby()
        {
            LobbyPlayerData playerData = new LobbyPlayerData(GetLocalID(), "HostPlayer");
            LobbyData lobbyData = new LobbyData();
            bool success = await LobbyManager.Instance.CreateLobby(maxPlayers, false, playerData.Serialize(), lobbyData.Serialize());
            return success;
        }

        public async Task<bool> JoinLobby(string code)
        {
            LobbyPlayerData playerData = new LobbyPlayerData(GetLocalID(), "JoinPlayer");
            bool success = await LobbyManager.Instance.JoinLobby(code, playerData.Serialize());
            return success;
        }

        public async Task<bool> DisconnectFromLobby()
        {
            try
            {
                string playerId = AuthenticationService.Instance.PlayerId;
                //await LobbyService.Instance.RemovePlayerAsync(LobbyManager.Instance.GetLobbyID, playerId);
                await LobbyManager.Instance.Disconnect();
                await SceneManager.LoadSceneAsync(Scenes.MenuScene.ToString());
                return true;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                return false;
            }
        }

        private async void OnLobbyUpdated(Lobby lobby)
        {
            List<Dictionary<string, PlayerDataObject>> players = LobbyManager.Instance.GetPlayersData();

            GameLobbyEvents.OnLobbyUpdated?.Invoke();

            if (LobbyUtil.NumberOfPlayersReady(players) == players.Count)
            {
                GameLobbyEvents.OnLobbyReady?.Invoke();
            }
            else
            {
                GameLobbyEvents.OnLobbyCancel?.Invoke();
            }

            if (LobbyManager.Instance.GetRelayCode() != null && !joined && !inGame)
            {
                await JoinRelayServer();
                await SceneManager.LoadSceneAsync("GameScene");
                joined = true;
            }
        }

        public async Task StartRelayServer()
        {
            string relayCode = await RelayManager.Instance.CreateRelay(maxPlayers);
            inGame = true;

            setTotalPlayersInTeams();

            LobbyData lobbyData = new LobbyData(relayCode, "GameScene", MatchDuration.matchDuration1);
            await LobbyManager.Instance.UpdateLobbyData(lobbyData.Serialize());

            Debug.Log("Hola desde el HOST");
            MatchInfo.Instance.AddNewPlayerConnectedServerRpc();
            MatchInfo.Instance.MatchDuration = LobbyManager.Instance.GetMatchDuration();

            string allocationId = RelayManager.Instance.GetAllocatorId();
            string connectionData = RelayManager.Instance.GetConnectionData();
            
            await SetLocalPlayerData(allocationId, connectionData);
            await SceneManager.LoadSceneAsync("GameScene");
        }

        private async Task<bool> JoinRelayServer()
        {
            await RelayManager.Instance.JoinRelay(LobbyManager.Instance.GetRelayCode());
            inGame = true;

            Debug.Log("Hola desde el CLIENT");
            MatchInfo.Instance.AddNewPlayerConnectedServerRpc();

            string allocationId = RelayManager.Instance.GetAllocatorId();
            string connectionData = RelayManager.Instance.GetConnectionData();

            await Task.Delay(200);
            await SetLocalPlayerData(allocationId, connectionData);

            return true;
        }

        private void setTotalPlayersInTeams()
        {
            int numberOfLocalPlayers = LobbyManager.Instance.GetNumberOfPlayersInTeams(PlayerTeam.Local);
            int numberOfVisitorPlayers = LobbyManager.Instance.GetNumberOfPlayersInTeams(PlayerTeam.Visitor);
            MatchInfo.Instance.NumberOfPlayersInTeams = numberOfLocalPlayers + numberOfVisitorPlayers;
        }

        private async Task SetLocalPlayerData(string allocationId = default, string connectionData = default)
        {
            LobbyPlayerData localPlayerData = LobbyUtil.DeserializePlayerDataWithID(GetLocalID());
            await LobbyManager.Instance.UpdatePlayerData(localPlayerData.Id, localPlayerData.Serialize(), allocationId, connectionData);
        }

        public async Task<bool> SetPlayerReady()
        {
            LobbyPlayerData localPlayerData = LobbyUtil.DeserializePlayerDataWithID(GetLocalID());
            localPlayerData.IsReady = true;
            return await LobbyManager.Instance.UpdatePlayerData(localPlayerData.Id, localPlayerData.Serialize());
        }

        public async Task<bool> SetPlayerNotReady()
        {
            LobbyPlayerData localPlayerData = LobbyUtil.DeserializePlayerDataWithID(GetLocalID());
            localPlayerData.IsReady = false;
            return await LobbyManager.Instance.UpdatePlayerData(localPlayerData.Id, localPlayerData.Serialize());
        }

        public async Task<bool> SetPlayerTeam(LobbyPlayerData playerData, PlayerTeam playerTeam)
        {
            playerData.PlayerTeam = playerTeam;
            return await LobbyManager.Instance.UpdatePlayerData(playerData.Id, playerData.Serialize());
        }

        public async Task<bool> SetMatchDuration(MatchDuration newMatchDuration)
        {
            Dictionary<string, DataObject> getLobbyData = LobbyManager.Instance.GetLobbyData();
            LobbyData lobbyData = new LobbyData(getLobbyData);
            lobbyData.MatchDuration = newMatchDuration;
            return await LobbyManager.Instance.UpdateLobbyData(lobbyData.Serialize());
        }

        public string GetLobbyCode()
        {
            return LobbyManager.Instance.GetLobbyCode();
        }

        public MatchDuration GetMatchDuration()
        {
            Dictionary<string, DataObject> lobbyData = LobbyManager.Instance.GetLobbyData();
            LobbyData getLobbyData = new LobbyData(lobbyData);
            return getLobbyData.MatchDuration;
        }

        public List<LobbyPlayerData> GetPlayerDataList()
        {
            List<LobbyPlayerData> players = new List<LobbyPlayerData>();
            List<Dictionary<string, PlayerDataObject>> playersData = LobbyManager.Instance.GetPlayersData();
            playersData.ForEach(playerData => players.Add(LobbyUtil.DeserializePlayerData(playerData)));
            return players;
        }

        public string GetLocalID() => AuthenticationService.Instance.PlayerId;
        public bool IsHost() => AuthenticationService.Instance.PlayerId == LobbyManager.Instance.GetHostID();
    }
}