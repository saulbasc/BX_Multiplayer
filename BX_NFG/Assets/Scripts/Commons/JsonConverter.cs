using Newtonsoft.Json;
using UnityEngine;

public static class JsonConverterUtil
{
    public static string ToJson<T>(T obj)
    {
        try
        {
            return JsonConvert.SerializeObject(obj);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[JSON ERROR] Error al serializar: {e.Message}");
            return null;
        }
    }

    public static T FromJson<T>(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[JSON ERROR] Error al deserializar: {e.Message}\nJSON: {json}");
            return default;
        }
    }
}
