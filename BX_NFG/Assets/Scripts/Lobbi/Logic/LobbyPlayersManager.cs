
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Datas;
using Assets.Scripts.Lobbi.Players;
using Assets.Scripts.Lobbi.Util;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Assets.Scripts.Lobbi.Logic
{
    public class LobbyPlayersManager : DefaultSingleton<LobbyPlayersManager>
    {
        private Lobby GetLobby() => LobbyDataManager.Instance.Lobby;
        public bool IsHost() => UnityServicesActions.GetCurrentUserID() == LobbyDataManager.Instance.GetHostID();
        public List<Player> GetPlayers() => GetLobby().Players;

        public List<Dictionary<string, PlayerDataObject>> GetPlayersData()
        {
            List<Dictionary<string, PlayerDataObject>> playersData = GetLobby()?.Players.Select(player => player.Data).ToList();
            return playersData;
        }

        public Dictionary<string, PlayerDataObject> GetSinglePlayerData(string playerId)
        {
            Dictionary<string, PlayerDataObject> playerData = GetLobby()?.Players.FirstOrDefault(player => player.Id == playerId)?.Data;  
            return playerData;
        }

        public LobbyPlayerData GetPlayerDataObject(string playerId)
        {
            Dictionary<string, PlayerDataObject> playerData = GetSinglePlayerData(playerId);
            return new LobbyPlayerData(playerData);
        }

        public PlayerTeam GetPlayerTeam (string playerId)
        {
            LobbyPlayerData playerData = GetPlayerDataObject(playerId);
            return playerData.PlayerTeam;
        }

        public async Task<bool> SetPlayerReadyAsync(bool ready)
        {
            try
            {
                LobbyPlayerData localPlayerData = GetPlayerDataObject(UnityServicesActions.GetCurrentUserID());
                localPlayerData.IsReady = ready;
                return await LobbyServiceHandler.Instance.UpdatePlayerData(localPlayerData.Id, localPlayerData.SerializeObjectToDictionary());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public async Task<bool> SetPlayerTeamAsync(LobbyPlayerData playerData, PlayerTeam playerTeam)
        {
            playerData.PlayerTeam = playerTeam;
            try
            {
                return await LobbyServiceHandler.Instance.UpdatePlayerData(playerData.Id, playerData.SerializeObjectToDictionary());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public List<LobbyPlayerData> GetPlayerDataList()
        {
            List<LobbyPlayerData> players = new List<LobbyPlayerData>();
            List<Dictionary<string, PlayerDataObject>> playersData = GetPlayersData();
            playersData.ForEach(playerData => players.Add(new LobbyPlayerData(playerData)));
            return players;
        }

        public async Task SetLocalPlayerData(string allocationId = default, string connectionData = default)
        {
            try
            {
                LobbyPlayerData localPlayerData = GetPlayerDataObject(UnityServicesActions.GetCurrentUserID());
                await LobbyServiceHandler.Instance.UpdatePlayerData(
                    localPlayerData.Id, 
                    localPlayerData.SerializeObjectToDictionary(), 
                    allocationId, 
                    connectionData
                );
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}