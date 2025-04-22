using System;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi.Players.Values
{
    public class EnumValueGetter<TEnum> : IValueGetter where TEnum : struct, Enum
    {
        public object GetValue(Dictionary<string, PlayerDataObject> data, string key)
        {
            if (data.TryGetValue(key, out var valueObj)
                && Enum.TryParse<TEnum>(valueObj.Value, true, out var result))
            {
                return result;
            }

            return default(TEnum);
        }
    }
}