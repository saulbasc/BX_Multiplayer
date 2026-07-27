
using System.Collections.Generic;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Assets.Scripts.Lobbi.Data;
using Newtonsoft.Json;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi.Datas
{
    /// <summary>
    /// Guarda los datos de la sala en un objeto serializable.
    /// </summary>
    public class LobbyData : SerializableLobbyModelBase<DataObject>
    {
        /// <summary>
        /// El código para unirse al partido.
        /// </summary>
        [JsonProperty(LobbyDataKeys.JoinRelayCode)]
        public string RelayJoinCode { get; set; }
        /// <summary>
        /// La duración del partido.
        /// </summary>
        [JsonProperty(LobbyDataKeys.MatchDuration)]
        public MatchDuration MatchDuration { get; set; }

        /// <summary>
        /// Constructor que crea un objeto con los datos aportados.
        /// </summary>
        /// <param name="relayJoinCode">El código para unirse al partido.</param>
        /// <param name="matchDuration">La duración del partido.</param>
        public LobbyData(string relayJoinCode, MatchDuration matchDuration)
        {
            RelayJoinCode = relayJoinCode;
            MatchDuration = matchDuration;
        }

        /// <summary>
        /// Constructor que deserializa directamente los datos de la sala.
        /// </summary>
        /// <param name="lobbyData"></param>
        public LobbyData(Dictionary<string, DataObject> lobbyData)
        {
            DeserializeFromDictionary(lobbyData);
        }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public LobbyData() { }

        protected override string GetValueAsString(DataObject dataObject)
        {
            return dataObject?.Value?.ToString();
        }
    }
}
