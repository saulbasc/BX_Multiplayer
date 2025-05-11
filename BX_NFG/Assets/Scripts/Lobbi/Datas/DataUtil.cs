
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi.Datas
{
    public static class DataUtil
    {
        public static Dictionary<string, PlayerDataObject> ToPlayerDataObjectDictionary(Dictionary<string, string> data)
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

        public static Dictionary<string, DataObject> ToLobbyDataObjectDictionary(Dictionary<string, string> data)
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
    }
}
