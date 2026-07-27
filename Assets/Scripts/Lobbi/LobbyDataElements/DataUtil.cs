
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi.Datas
{
    /// <summary>
    /// Clase utilitaria para convertir datos clave-valor en estructuras de datos del Lobby.
    /// </summary>
    public static class DataUtil
    {
        /// <summary>
        /// Convierte un diccionario de claves y valores en strings a un diccionario de PlayerDataObject/>,
        /// </summary>
        /// <param name="data">Datos string a convertir.</param>
        /// <returns>Un diccionario donde cada clave se asocia a un objeto PlayerDataObject/>.</returns>
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

        /// <summary>
        /// Convierte un diccionario de claves y valores en strings a un diccionario de DataObject/>,
        /// </summary>
        /// <param name="data">Datos string a convertir.</param>
        /// <returns>Un diccionario donde cada clave se asocia a un objeto PlayerDataObject/>.</returns>
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
