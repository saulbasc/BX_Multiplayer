
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Lobbi.Players;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Assets.Scripts.Lobbi.Util
{
    public static class LobbyUtil
    {
        public static Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
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

        public static Dictionary<string, DataObject> SerializeLobbyData(Dictionary<string, string> data)
        {
            Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>();
            foreach (var (key, value) in data)
            {
                lobbyData.Add(key, new DataObject(
                    visibility: DataObject.VisibilityOptions.Member,
                    value: value
                ));
            }
            return lobbyData;
        }

        public static LobbyPlayerData DeserializePlayerData(Dictionary<string, PlayerDataObject> data)
        {
            LobbyPlayerData playerData = new LobbyPlayerData(data);
            return playerData;
        }

        public static LobbyPlayerData DeserializePlayerDataWithID(string playerID)
        {
            Dictionary<string, PlayerDataObject> data = LobbyPlayersManager.Instance.GetSinglePlayerData(playerID);
            LobbyPlayerData playerData = new LobbyPlayerData(data);
            return playerData;
        }

        public static int NumberOfPlayersReady(List<Dictionary<string, PlayerDataObject>> players)
        {
            int numberOfPlayersReady = 0;
            players.ForEach(player => { numberOfPlayersReady = SumPlayerReady(player, numberOfPlayersReady); });
            return numberOfPlayersReady;
        }

        public static int SumPlayerReady(Dictionary<string, PlayerDataObject> playerData, int playersReady)
        {
            LobbyPlayerData player = new LobbyPlayerData(playerData);
            return player.IsReady
                ? playersReady + 1
                : playersReady;
        }
    }
}
