
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Assets.Scripts.Commons;
using Assets.Scripts.Lobbi.Util;
using System;
using Assets.Scripts.Lobbi.Data;
using System.Linq;

namespace Assets.Scripts.Lobbi
{
    public class LobbyManager : Singleton<LobbyManager>
    {
        private Lobby lobby;
        private Coroutine lobbyCoroutine;
        private Coroutine refreshLobbyCoroutine;

        public async Task<bool> CreateLobby(int maxPlayers, bool isPrivate, Dictionary<string, string> data, Dictionary<string, string> lobbyData)
        {
            Dictionary<string, PlayerDataObject> playerData = LobbyUtil.SerializePlayerData(data);
            Dictionary<string, DataObject> lobbyDataSerialized = LobbyUtil.SerializeLobbyData(lobbyData);
            Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
                Player = player,
                Data = lobbyDataSerialized,
            };

            try {
                lobby = await LobbyService.Instance.CreateLobbyAsync("MyLobby", maxPlayers, lobbyOptions);
                lobbyCoroutine = StartCoroutine(LobbyCoroutine(lobby.Id, 2f));
                refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 1f));
                return true;
            }
            catch (Exception)
            { 
                return false; 
            }
        }

        public async Task<bool> JoinLobby(string code, Dictionary<string, string> playerData)
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
            Player player = new Player(AuthenticationService.Instance.PlayerId, null, LobbyUtil.SerializePlayerData(playerData));
            options.Player = player;

            try {
                lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
                refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 1f));
                return true;
            }
            catch (Exception e) 
            {
                Debug.Log(e);
                return false;
            }
        }

        public void QuitLobby()
        {
            if (lobby != null && lobby.HostId == AuthenticationService.Instance.PlayerId)
            {
                LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
            }
        }

        private IEnumerator RefreshLobbyCoroutine(string lobbyId, float wait)
        {
            while (true)
            {
                Task<Lobby> task = LobbyService.Instance.GetLobbyAsync(lobbyId);
                yield return new WaitUntil(() => task.IsCompleted);
                Lobby newLobby = task.Result;
                if(newLobby.LastUpdated > lobby.LastUpdated)
                {
                    lobby = newLobby;
                    LobbyEvents.OnLobbyUpdated?.Invoke(lobby);
                }
                yield return new WaitForSecondsRealtime(wait);
            }
        }

        public static IEnumerator LobbyCoroutine(string lobbyId, float wait)
        {
            while (true)
            {
                Debug.Log("Lobby coroutine");
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
                yield return new WaitForSecondsRealtime(wait);
            }
        }

        public async Task<bool> UpdatePlayerData(string id, Dictionary<string, string> data, string allocationId = default, string connectionData = default)
        {
            UpdatePlayerOptions options = new UpdatePlayerOptions 
            { 
                Data = LobbyUtil.SerializePlayerData(data),
                AllocationId = allocationId,
                ConnectionInfo = connectionData,
            };

            try { 
                lobby = await LobbyService.Instance.UpdatePlayerAsync(lobby.Id, id, options);
                LobbyEvents.OnLobbyUpdated(lobby);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public async Task<bool> UpdateLobbyData(Dictionary<string, string> data)
        {
            UpdateLobbyOptions options = new UpdateLobbyOptions
            {
                Data = LobbyUtil.SerializeLobbyData(data),
            };

            try
            {
                lobby = await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, options);
                LobbyEvents.OnLobbyUpdated(lobby);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public List<Dictionary<string, PlayerDataObject>> GetPlayersData()
        {
            List<Dictionary<string, PlayerDataObject>> playersData = new List<Dictionary<string, PlayerDataObject>>();
            lobby.Players.ForEach(player => playersData.Add(player.Data));
            return playersData;
        }

        public Dictionary<string, PlayerDataObject> GetPlayerData(string id)
        {
            return lobby.Players.FirstOrDefault(player => player.Id == id)?.Data;
        }

        public string GetRelayCode()
        {
            return lobby != null && lobby.Data != null && lobby.Data.ContainsKey(LobbyDataKeys.JoinRelayCode)
                ? lobby.Data[LobbyDataKeys.JoinRelayCode].Value
                : null;
        }

        public Dictionary<string, DataObject> GetLobbyData() => lobby.Data;

        public string GetLobbyCode() => lobby?.LobbyCode;

        public string GetHostID() => lobby?.HostId;
    }
}