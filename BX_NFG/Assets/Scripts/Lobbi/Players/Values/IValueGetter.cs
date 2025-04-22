
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi.Players.Values
{
    public interface IValueGetter
    {
        object GetValue(Dictionary<string, PlayerDataObject> data, string key);
    }
}
