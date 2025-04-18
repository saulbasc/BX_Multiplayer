
using System.Collections;
using System.Collections.Generic;
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
