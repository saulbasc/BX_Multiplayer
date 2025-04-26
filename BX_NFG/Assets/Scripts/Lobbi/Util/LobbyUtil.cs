
using System;
using System.Collections;
using System.Collections.Generic;
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
            Dictionary<string, PlayerDataObject> data = LobbyManager.Instance.GetPlayerData(playerID);
            LobbyPlayerData playerData = new LobbyPlayerData(data);
            return playerData;
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
    }
}
