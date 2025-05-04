using Assets.Scripts.GameManager.GameEvents.Timer;
using Assets.Scripts.Lobbi.Data;
using System.Collections.Generic;
using System;
using Unity.Services.Lobbies.Models;
using Assets.Scripts.Commons;
using System.Threading.Tasks;
using Assets.Scripts.Lobbi.Datas;
using UnityEngine;
using Assets.Scripts.Game.Manager;
using System.Linq;

namespace Assets.Scripts.Lobbi.Logic
{
    public class LobbyDataManager : DefaultSingleton<LobbyDataManager>
    {
        public Lobby Lobby { get; private set; }
        public void SetLobby(Lobby lobby) => Lobby = lobby;

        public string GetLobbyID() => Lobby.Id;

        public string GetLobbyCode() => Lobby.LobbyCode;

        public Dictionary<string, DataObject> GetLobbyData() => Lobby.Data;

        public string GetHostID() => Lobby.HostId;

        public string GetRelayCode()
        {
            return Lobby.Data != null && Lobby.Data.ContainsKey(LobbyDataKeys.JoinRelayCode)
                ? Lobby.Data[LobbyDataKeys.JoinRelayCode].Value
                : null;
        }

        public MatchDuration GetMatchDuration()
        {
            if (Lobby.Data != null && Lobby.Data.ContainsKey(LobbyDataKeys.MatchDuration))
            {
                return (MatchDuration)Enum.Parse(typeof(MatchDuration), Lobby.Data[LobbyDataKeys.MatchDuration].Value);
            }
            return MatchDuration.matchDuration1;
        }

        public async Task<bool> SetMatchDurationAsync(MatchDuration newMatchDuration)
        {
            try
            {
                Dictionary<string, DataObject> getLobbyData = Lobby.Data;
                LobbyData lobbyData = new LobbyData(getLobbyData);
                lobbyData.MatchDuration = newMatchDuration;
                return await LobbyServiceHandler.Instance.UpdateLobbyData(lobbyData.Serialize());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public int GetNumberOfPlayersInTeams(PlayerTeam playerTeam)
        {
            List<Dictionary<string, PlayerDataObject>> playersData = LobbyPlayersManager.Instance.GetPlayersData();
            return playersData.Count(playerData
                => playerData.TryGetValue(PlayerDataKeys.PlayerTeam, out var teamObj)
                && teamObj.Value == playerTeam.ToString());
        }

        public void SetTotalPlayersInTeams()
        {
            int numberOfLocalPlayers = GetNumberOfPlayersInTeams(PlayerTeam.Local);
            int numberOfVisitorPlayers = GetNumberOfPlayersInTeams(PlayerTeam.Visitor);
            MatchInfo.Instance.SetNumberOfPlayersInTeams(numberOfLocalPlayers + numberOfVisitorPlayers); 
        }
    }
}