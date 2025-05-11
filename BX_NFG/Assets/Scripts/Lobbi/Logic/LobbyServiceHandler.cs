
using Assets.Scripts.Lobbi.Util;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using Assets.Scripts.Commons;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Assets.Scripts.Init;
using Assets.Scripts.Connection.Lobbi;
using Assets.Scripts.Lobbi.Datas;

namespace Assets.Scripts.Lobbi.Logic
{
    public class LobbyServiceHandler : DefaultSingleton<LobbyServiceHandler>
    {
        public async Task<bool> CreateLobby(int maxPlayers, bool isPrivate, Dictionary<string, string> getPlayerData, Dictionary<string, string> getLobbyData)
        {
            Dictionary<string, PlayerDataObject> playerData = DataUtil.ToPlayerDataObjectDictionary(getPlayerData);
            Dictionary<string, DataObject> lobbyDataSerialized = DataUtil.ToLobbyDataObjectDictionary(getLobbyData);
            Player player = new Player(UnityServicesActions.GetCurrentUserID(), null, playerData);

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
                Player = player,
                Data = lobbyDataSerialized,
            };

            try
            {
                Lobby newLobby = await LobbyService.Instance.CreateLobbyAsync("MyLobby", maxPlayers, lobbyOptions);
                LobbyDataManager.Instance.SetLobby(newLobby);
                LobbyUpdaterManager.Instance.StartUpdating(LobbyDataManager.Instance.GetLobbyID(), 1f);
                LobbyHeartbeatManager.Instance.StartHeartbeatCororutine(LobbyDataManager.Instance.GetLobbyID(), 5f);
                return true;
            }
            catch (Exception e)
            {
                 Debug.LogError(e);
                return false;
            }
        }

        public async Task<bool> JoinLobby(string code, Dictionary<string, string> getPlayerData)
        {
            Dictionary<string, PlayerDataObject> playerData = DataUtil.ToPlayerDataObjectDictionary(getPlayerData);
            Player player = new Player(UnityServicesActions.GetCurrentUserID(), null, playerData);

            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions
            {
                Player = player,
            };

            try
            {
                Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
                LobbyDataManager.Instance.SetLobby(lobby);
                LobbyUpdaterManager.Instance.StartUpdating(LobbyDataManager.Instance.GetLobbyID(), 1f);
                LobbyHeartbeatManager.Instance.StartHeartbeatCororutine(LobbyDataManager.Instance.GetLobbyID(), 5f);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public async Task<bool> UpdatePlayerData(string id, Dictionary<string, string> playerData, string allocationId = default, string connectionData = default)
        {
            UpdatePlayerOptions options = new UpdatePlayerOptions
            {
                Data = DataUtil.ToPlayerDataObjectDictionary(playerData),
                AllocationId = allocationId,
                ConnectionInfo = connectionData,
            };

            try
            {
                await LobbyService.Instance.UpdatePlayerAsync(LobbyDataManager.Instance.GetLobbyID(), id, options);
                LobbyEvents.OnLobbyUpdated(LobbyDataManager.Instance.Lobby);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public async Task<bool> UpdateLobbyData(Dictionary<string, string> lobbyData)
        {
            UpdateLobbyOptions options = new UpdateLobbyOptions
            {
                Data = DataUtil.ToLobbyDataObjectDictionary(lobbyData),
            };

            try
            {
                await LobbyService.Instance.UpdateLobbyAsync(LobbyDataManager.Instance.GetLobbyID(), options);
                LobbyEvents.OnLobbyUpdated(LobbyDataManager.Instance.Lobby);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public async Task Disconnect()
        {
            StopAllUpdaters();
            try
            {
                if (UnityServicesActions.GetCurrentUserID() == LobbyDataManager.Instance.GetHostID())
                {
                    await HostDisconnection();
                }
                else
                {
                    await LobbyService.Instance.RemovePlayerAsync(LobbyDataManager.Instance.GetLobbyID(), UnityServicesActions.GetCurrentUserID());
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async Task HostDisconnection()
        {
            List<Player> players = LobbyPlayersManager.Instance.GetPlayers();
            var newHost = players.FirstOrDefault(player => player.Id != UnityServicesActions.GetCurrentUserID());
            if (newHost != null)
            {
                try
                {
                    await LobbyService.Instance.UpdateLobbyAsync(LobbyDataManager.Instance.GetLobbyID(), new UpdateLobbyOptions
                    {
                        HostId = newHost.Id
                    });
                    await LobbyService.Instance.RemovePlayerAsync(LobbyDataManager.Instance.GetLobbyID(), UnityServicesActions.GetCurrentUserID());
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                await LobbyService.Instance.DeleteLobbyAsync(LobbyDataManager.Instance.GetLobbyID());
            }
        }

        public async Task<bool> DisconnectFromLobby()
        {
            try
            {
                string playerId = UnityServicesActions.GetCurrentUserID();
                await Disconnect();
                await SceneManager.LoadSceneAsync(Scenes.MenuScene.ToString());
                return true;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                return false;
            }
        }

        public void StopAllUpdaters()
        {
            try
            {
                LobbyUpdaterManager.Instance.StopUpdating();
                LobbyHeartbeatManager.Instance.StopHeartbeatCoroutine();
                LobbyUpdaterManager.Instance.Delete();
                LobbyHeartbeatManager.Instance.Delete();
                LobbyManager.Instance.Delete();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void LobbyClosed()
        {
            StopAllUpdaters();
        }
    }
}