using Assets.Scripts.GameManager.GameEvents.Timer;
using Assets.Scripts.Lobbi.Data;
using System.Collections.Generic;
using System;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using Assets.Scripts.Lobbi.Datas;
using System.Linq;
using Assets.Scripts.Init;
using Assets.Scripts.Handlers;
using Unity.Services.Lobbies;
using Assets.Scripts.Lobbi.Players;
using UnityEngine;

namespace Assets.Scripts.Lobbi.Logic
{
    /// <summary>
    /// Clase encargada de gestionar los datos de la Lobby.
    /// </summary>
    public class LobbyDataManager : MonoBehaviour
    {
        [SerializeField] private LobbyPlayerManager lobbyPlayerManager;
        /// <summary>
        /// El objeto Lobby con el que se trabaja.
        /// </summary>
        public Lobby Lobby { get; private set; }
        public void SetLobby(Lobby lobby) => Lobby = lobby;
        public string GetLobbyID() => Lobby.Id;
        public string GetLobbyCode() => Lobby.LobbyCode;
        public string GetHostID() => Lobby.HostId;

        /// <summary>
        /// Obtiene la lista de jugadores de la Lobby.
        /// </summary>
        /// <returns>La lista de jugadores de la Lobby.</returns>
        public List<Player> GetPlayers() => Lobby.Players;
        /// <summary>
        /// Obtiene el número actual de jugadores totales que tiene la Lobby.
        /// </summary>
        /// <returns>El número total de jugadores.</returns>
        public int GetNumberOfPlayers() => Lobby.Players.Count;
        /// <summary>
        /// Comprueba si el usuario local es el host de la Lobby.
        /// </summary>
        /// <returns>True si el usuario local es host.</returns>
        public bool IsLocalPlayerHost() => UnityServicesActions.GetCurrentUserID() == Lobby.HostId;
        /// <summary>
        /// Obtiene los datos de la Lobby en formato string y DataObject.
        /// </summary>
        /// <returns>El diccionario con los datos de la Lobby.</returns>
        public Dictionary<string, DataObject> GetLobbyDataObjectDictionary() => Lobby.Data;
        /// <summary>
        /// Obtiene los datos de la Lobby en formato LobbyData.
        /// </summary>
        /// <returns>El objeto LobbyData con los datos de la Lobby.</returns>
        public LobbyData GetLobbyDataObject() => new LobbyData(GetLobbyDataObjectDictionary());

        /// <summary>
        /// Obtiene el código del Relay para acceder al partido guardado en la Lobby.
        /// </summary>
        /// <returns>El código del Relay en formato string</returns>
        public string GetLobbyRelayCode()
        {
            return Lobby.Data != null && Lobby.Data.ContainsKey(LobbyDataKeys.JoinRelayCode)
                ? Lobby.Data[LobbyDataKeys.JoinRelayCode].Value
                : null;
        }

        /// <summary>
        /// Obtiene la duración del partido guardao en la Lobby.
        /// </summary>
        /// <returns>Enum con la duración del partido</returns>
        public MatchDuration GetLobbyMatchDuration()
        {
            if (Lobby.Data != null && Lobby.Data.ContainsKey(LobbyDataKeys.MatchDuration))
            {
                return (MatchDuration)Enum.Parse(typeof(MatchDuration), Lobby.Data[LobbyDataKeys.MatchDuration].Value);
            }
            return MatchDuration.matchDuration1;
        }

        /// <summary>
        /// Establece una nueva duración de partido en los datos de la Lobby.
        /// </summary>
        /// <param name="newMatchDuration">Enum de la nueva duración del partido.</param>
        /// <returns>True si se cambia la duración del partido correctamente.</returns>
        public async Task<bool> SetLobbyMatchDurationAsync(MatchDuration newMatchDuration)
        {
            Dictionary<string, DataObject> getLobbyData = Lobby.Data;
            LobbyData lobbyData = new LobbyData(getLobbyData);
            lobbyData.MatchDuration = newMatchDuration;
            return await UpdateLobbyData(lobbyData.SerializeObjectToDictionary());
        }

        /// <summary>
        /// Obetener el número de jugadores registrados en la Lobby.
        /// </summary>
        /// <param name="playerTeam">Enum del equipo a buscar.</param>
        /// <returns>El número de jugadores en el equipo.</returns>
        public int GetNumberOfPlayersInLobbyTeams(PlayerTeam playerTeam)
        {
            List<Dictionary<string, PlayerDataObject>> playersData = lobbyPlayerManager.GetAllPlayersData();

            return playersData.Count(playerData =>
                playerData.TryGetValue(PlayerDataKeys.PlayerTeam, out var teamObj)
                && teamObj.Value == ((int)playerTeam).ToString());
        }

        /// <summary>
        /// Actualiza los datos de la Lobby.
        /// </summary>
        /// <param name="lobbyData">Los datos de la lobby en formato string.</param>
        /// <returns>True si se actualiza correctamente.</returns>
        public async Task<bool> UpdateLobbyData(Dictionary<string, string> lobbyData)
        {
            UpdateLobbyOptions options = new UpdateLobbyOptions
            {
                Data = DataUtil.ToLobbyDataObjectDictionary(lobbyData),
            };

            return await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                Lobby newLobby = await LobbyService.Instance.UpdateLobbyAsync(Lobby.Id, options);
                Lobby = newLobby;
                LobbyEvents.Instance.RaiseNewLobbyUpdated(Lobby);
                return true;
            }, false);
        }

        /// <summary>
        /// Calcula el número de jugadores que están listos en la Lobby.
        /// </summary>
        /// <returns>El número de jugadores que están listos en la Lobby.</returns>
        public int NumberOfPlayersReady()
        {
            List<Dictionary<string, PlayerDataObject>> players = lobbyPlayerManager.GetAllPlayersData();
            int numberOfPlayersReady = 0;
            players.ForEach(player => { numberOfPlayersReady = SumPlayerReady(player, numberOfPlayersReady); });
            return numberOfPlayersReady;
        }

        private int SumPlayerReady(Dictionary<string, PlayerDataObject> playerData, int playersReady)
        {
            LobbyPlayerData player = new LobbyPlayerData(playerData);
            return player.IsReady
                ? playersReady + 1
                : playersReady;
        }
    }
}