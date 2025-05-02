
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
using Assets.Scripts.GameManager.GameEvents.Timer;

namespace Assets.Scripts.Lobbi
{
    public class LobbyManager : Singleton<LobbyManager>
    {
        private Lobby lobby;
        private Coroutine lobbyCoroutine;
        private Coroutine refreshLobbyCoroutine;

        private void OnDisable()
        {
            Destroy(gameObject);
        }

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
            catch (Exception e)
            {
                Debug.LogError(e);
                return false; 
            }
        }

        public async Task<bool> JoinLobby(string code, Dictionary<string, string> playerData)
        {
            Debug.Log("Lobby introduced code => " + code);
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
                Debug.LogError(e);
                return false;
            }
        }

        private IEnumerator RefreshLobbyCoroutine(string lobbyId, float wait)
        {
            while (true)
            {
                yield return StartCoroutine(TryUpdateLobby(lobbyId));
                yield return new WaitForSecondsRealtime(wait);
            }
        }

        private IEnumerator TryUpdateLobby(string lobbyId)
        {
            Task<Lobby> task = null;

            try
            {
                task = LobbyService.Instance.GetLobbyAsync(lobbyId);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                yield break;
            }

            yield return new WaitUntil(() => task.IsCompleted);

            if (task.IsCompletedSuccessfully)
            {
                HandleLobbyResult(task.Result);
            }
            else
            {
                Debug.LogError($"Error al obtener el lobby: {task.Exception?.Flatten().InnerException}");
            }
        }

        private void HandleLobbyResult(Lobby newLobby)
        {
            try
            {
                if (newLobby.LastUpdated > lobby.LastUpdated)
                {
                    lobby = newLobby;
                    LobbyEvents.OnLobbyUpdated?.Invoke(lobby);
                    Debug.Log($"Host actual: {lobby?.HostId}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public static IEnumerator LobbyCoroutine(string lobbyId, float wait)
        {
            while (true)
            {
                try
                {
                    LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                yield return new WaitForSecondsRealtime(wait);
            }
        }

        private void DeleteCorroutines()
        {
            if (lobbyCoroutine != null)
            {
                StopCoroutine(lobbyCoroutine);
                lobbyCoroutine = null;
            }
            if (refreshLobbyCoroutine != null)
            {
                StopCoroutine(refreshLobbyCoroutine);
                refreshLobbyCoroutine = null;
            }
        }

        public async Task Disconnect()
        {
            try
            {
                DeleteCorroutines();

                if (AuthenticationService.Instance.PlayerId == lobby.HostId)
                {
                    var newHost = lobby.Players.FirstOrDefault(player => player.Id != AuthenticationService.Instance.PlayerId);
                    if (newHost != null)
                    {
                        await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, new UpdateLobbyOptions
                        {
                            HostId = newHost.Id
                        });
                        Debug.Log($"Nuevo host asignado: {newHost.Id}");
                        await LobbyService.Instance.RemovePlayerAsync(lobby.Id, AuthenticationService.Instance.PlayerId);

                    }
                    else
                    {
                        await LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
                    }
                }
                else
                {
                    await LobbyService.Instance.RemovePlayerAsync(lobby.Id, AuthenticationService.Instance.PlayerId);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }


        public void LobbyClosed()
        {
            try
            {
                DeleteCorroutines();
                lobby = null;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
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

        public int GetNumberOfPlayersInTeams(PlayerTeam playerTeam)
        {
            int numberOfPlayers = 0;
            List<Dictionary<string, PlayerDataObject>> playersData = GetPlayersData();
            playersData.ForEach(playerData =>
            {
                if (playerData.ContainsKey(PlayerDataKeys.PlayerTeam) && playerData[PlayerDataKeys.PlayerTeam].Value == playerTeam.ToString())
                {
                    numberOfPlayers++;
                }
            });
            return numberOfPlayers;
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

        public string GetLobbyID => lobby.Id;

        public string GetLobbyCode() => lobby?.LobbyCode;

        public string GetHostID() => lobby?.HostId;

        public MatchDuration GetMatchDuration()
        {
            if (lobby.Data != null && lobby.Data.ContainsKey(LobbyDataKeys.MatchDuration))
            {
                return (MatchDuration)Enum.Parse(typeof(MatchDuration), lobby.Data[LobbyDataKeys.MatchDuration].Value);
            }
            return MatchDuration.matchDuration1;
        }
    }
}