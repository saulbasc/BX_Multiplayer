using System.Collections.Generic;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Datas;
using Newtonsoft.Json;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi.Players
{
    /// <summary>
    /// Guarda los datos de un jugador de la sala en un objeto serializable.
    /// </summary>
    public class LobbyPlayerData : SerializableLobbyModelBase<PlayerDataObject>
    {
        /// <summary>
        /// El id del usuario.
        /// </summary>
        [JsonProperty(PlayerDataKeys.Id)]
        public string Id { get; set; }

        /// <summary>
        /// El nombre del usuario.
        /// </summary>
        [JsonProperty(PlayerDataKeys.GameTag)]
        public string GameTag { get; set; }

        /// <summary>
        /// Si el usuario está listo para jugar el partido.
        /// </summary>
        [JsonProperty(PlayerDataKeys.IsReady)]
        public bool IsReady { get; set; }

        /// <summary>
        /// El equipo al que pertenece el usuario en el partido.
        /// </summary>
        [JsonProperty(PlayerDataKeys.PlayerTeam)]
        public PlayerTeam PlayerTeam { get; set; }

        /// <summary>
        /// Constructor directo con propiedades.
        /// </summary>
        public LobbyPlayerData(string id, string gameTag)
        {
            Id = id;
            GameTag = gameTag;
            IsReady = false;
            PlayerTeam = PlayerTeam.Spectator;
        }

        /// <summary>
        /// Constructor que deserializa desde datos del lobby.
        /// </summary>
        public LobbyPlayerData(Dictionary<string, PlayerDataObject> playerData)
        { 
            DeserializeFromDictionary(playerData);
        }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public LobbyPlayerData() { }

        protected override string GetValueAsString(PlayerDataObject dataObject)
        {
            return dataObject.Value?.ToString();
        }
    }
}
