using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Assets.Scripts.Lobbi;
using Assets.Scripts.Lobbi.Data;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Assets.Scripts.Connection.Lobbi
{
    public class GameLobbyManager : Singleton<GameLobbyManager>
    {
        private List<LobbyPlayerData> playersData = new List<LobbyPlayerData>();
        private LobbyPlayerData localPlayerData;

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
            playerData.Inizialize(AuthenticationService.Instance.PlayerId, "HostPlayer");
            bool success = await LobbyManager.Instance.CreateLobby(4, false, playerData.Serialize());
            return success;
        }

        public async Task<bool> JoinLobby(string code)
        {
            LobbyPlayerData playerData = new LobbyPlayerData();
            playerData.Inizialize(AuthenticationService.Instance.PlayerId, "JoinPlayer");
            bool success = await LobbyManager.Instance.JoinLobby(code, playerData.Serialize());
            return success;
        }

        private void OnLobbyUpdated(Lobby lobby)
        {
            List<Dictionary<string, PlayerDataObject>> players = LobbyManager.Instance.GetPlayersData();
            playersData.Clear();

            foreach (Dictionary<string, PlayerDataObject> data in players)
            {
                LobbyPlayerData playerData = new LobbyPlayerData();
                playerData.Inizialice(data);

                if (AuthenticationService.Instance.PlayerId == playerData.Id)
                {
                    localPlayerData = playerData;
                }

                playersData.Add(playerData);
            }

            GameLobbyEvents.OnLobbyUpdated?.Invoke();
        }

        public string GetLobbyCode()
        {
            return LobbyManager.Instance.GetLobbyCode();
        }

        public List<LobbyPlayerData> GetPlayerDataList()
        {
            return playersData;
        }

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

        public async Task<bool> SetPlayerTeam(PlayerTeam team)
        {
            localPlayerData.PlayerTeam = team;
            return await LobbyManager.Instance.UpdatePlayerData(localPlayerData.Id, localPlayerData.Serialize());
        }
    }
}
