
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class LobbyManager : Singleton<LobbyManager>
    {
        private Lobby lobby;
        private Coroutine lobbyCoroutine;
        private Coroutine refreshLobbyCoroutine;

        public async Task<bool> CreateLobby(int maxPlayers, bool isPrivate, Dictionary<string, string> data)
        {
            Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);
            Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = player,
            };
            try
            {
                lobby = await LobbyService.Instance.CreateLobbyAsync("MyLobby", maxPlayers, lobbyOptions);
            }
            catch (System.Exception)
            {
                return false;
            }

            lobbyCoroutine = StartCoroutine(LobbyCoroutine(lobby.Id, 6f));
            refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 1f));

            Debug.Log("Lobby created with id: " + lobby.Id);

            return true;
        }

        private IEnumerator LobbyCoroutine(string lobbyId, float wait)
        {
            while (true)
            {
                Debug.Log("Lobby coroutine");
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
                yield return new WaitForSecondsRealtime(wait);
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
                }
                yield return new WaitForSecondsRealtime(wait);
            }
        }

        private Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
        {
            Dictionary<string, PlayerDataObject> playerData = new Dictionary<string, PlayerDataObject>();
            foreach (var (key, value) in data)
            {
                playerData.Add(key, new PlayerDataObject(
                    visibility: PlayerDataObject.VisibilityOptions.Member,
                    value: value
                ));
            }
            return playerData;
        }

        public void OnQuit()
        {
            if(lobby != null && lobby.HostId == AuthenticationService.Instance.PlayerId)
            {
                LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
            }
        }

        public string GetLobbyCode()
        {
            return lobby?.LobbyCode;
        }

        public async Task<bool> JoinLobby(string code, Dictionary<string, string> playerData)
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
            Player player = new Player(AuthenticationService.Instance.PlayerId, null, SerializePlayerData(playerData));
            options.Player = player;
            try
            {
                lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
            }
            catch (System.Exception)
            {
                return false;
            }

            StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 1f));
            return true;
        }
    }
}