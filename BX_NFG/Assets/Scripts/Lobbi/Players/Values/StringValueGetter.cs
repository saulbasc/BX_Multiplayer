
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi.Players.Values
{
    public class StringValueGetter : IValueGetter
    {
        public object GetValue(Dictionary<string, PlayerDataObject> data, string key)
        {
            return data.TryGetValue(key, out var valueObj) ? valueObj.Value : string.Empty;
        }
    }
}
