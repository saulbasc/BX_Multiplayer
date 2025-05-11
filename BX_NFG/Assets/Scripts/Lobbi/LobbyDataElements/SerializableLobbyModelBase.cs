using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Lobbi.Datas
{
    /// <summary>
    /// Base para la implementación de objeto serializable
    /// </summary>
    /// <typeparam name="TDataObject">El tipo de valor de los diccionarios aceptados por la clase</typeparam>
    public abstract class SerializableLobbyModelBase<TDataObject> : ISerializableLobbyModel<TDataObject>
    {
        /// <summary>
        /// Transforma el valor obtenido en un string
        /// </summary>
        /// <param name="dataObject">El valor a transformar</param>
        /// <returns>El valor en forma de string</returns>
        protected abstract string GetValueAsString(TDataObject dataObject);

        public void DeserializeFromDictionary(Dictionary<string, TDataObject> data)
        {
            if (data == null)
            {
                Debug.LogWarning("Lobby data is null");
                return;
            }

            try
            {
                var dataMap = data.ToDictionary(
                    kvp => kvp.Key,
                    kvp => GetValueAsString(kvp.Value)
                 );
                string json = JsonConvert.SerializeObject(dataMap);
                JsonConvert.PopulateObject(json, this);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to deserialize LobbyData: {ex.Message}");
            }
        }

        public Dictionary<string, string> SerializeObjectToDictionary()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to serialize LobbyData: {ex.Message}");
                return new Dictionary<string, string>();
            }
        }
    }
}
