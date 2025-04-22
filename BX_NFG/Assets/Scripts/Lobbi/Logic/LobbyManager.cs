
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

namespace Assets.Scripts.Lobbi
{
    public class LobbyManager : Singleton<LobbyManager>
    {
        private Lobby lobby;
        private Coroutine lobbyCoroutine;
        private Coroutine refreshLobbyCoroutine;

        //---------------------------

        public async Task<bool> CreateLobby(int maxPlayers, bool isPrivate, Dictionary<string, string> data)
        {
            Dictionary<string, PlayerDataObject> playerData = LobbyUtil.SerializePlayerData(data);
            Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
                Player = player,
            };

            try {lobby = await LobbyService.Instance.CreateLobbyAsync("MyLobby", maxPlayers, lobbyOptions);}
            catch (Exception){ return false; }

            lobbyCoroutine = StartCoroutine(LobbyUtil.LobbyCoroutine(lobby.Id, 6f));
            refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 1f));

            return true;
        }

        public async Task<bool> JoinLobby(string code, Dictionary<string, string> playerData)
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
            Player player = new Player(AuthenticationService.Instance.PlayerId, null, LobbyUtil.SerializePlayerData(playerData));
            options.Player = player;

            try { lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options); }
            catch (Exception e) 
            {
                Debug.Log(e);
                return false;
            }

            if (refreshLobbyCoroutine != null)
                StopCoroutine(refreshLobbyCoroutine);

            refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobby.Id, 1f));

            return true;
        }

        public void QuitLobby()
        {
            if (lobby != null && lobby.HostId == AuthenticationService.Instance.PlayerId)
            {
                LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
            }
        }

        //---------------------------

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

        //---------------------------

        public async Task<bool> UpdatePlayerData(string id, Dictionary<string, string> data)
        {
            Dictionary<string, PlayerDataObject> playerData = LobbyUtil.SerializePlayerData(data);
            UpdatePlayerOptions options = new UpdatePlayerOptions { Data = playerData };

            try { lobby = await LobbyService.Instance.UpdatePlayerAsync(lobby.Id, id, options); }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }

            LobbyEvents.OnLobbyUpdated(lobby);
            return true;
        }

        //---------------- GETTERS ----------------

        public List<Dictionary<string, PlayerDataObject>> GetPlayersData()
        {
            List<Dictionary<string, PlayerDataObject>> playersData = new List<Dictionary<string, PlayerDataObject>>();
            lobby.Players.ForEach(player => playersData.Add(player.Data));
            return playersData;
        }

        public string GetLobbyCode()
        {
            return lobby?.LobbyCode;
        }

        public string GetHostID()
        {
            return lobby?.HostId;
        }
    }
}