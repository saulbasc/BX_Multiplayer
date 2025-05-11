using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Assets.Scripts.Handlers;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Datas;
using Assets.Scripts.Lobbi.Players;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi.Logic
{
    /// <summary>
    /// Clase que gestiona los datos de los jugadores en la Lobby.
    /// </summary>
    public class LobbyPlayersManager : DefaultSingleton<LobbyPlayersManager>
    {
        /// <summary>
        /// Obtiene los datos de todos los jugadores de la Lobby en formato string y PlayerDataObject.
        /// </summary>
        /// <returns>El diccionario con los datos de todos los jugadores.</returns>
        public List<Dictionary<string, PlayerDataObject>> GetAllPlayersData()
        {
            return LobbyDataManager.Instance.GetPlayers().Select(player => player.Data).ToList();
        }

        /// <summary>
        /// Obtiene los datos de todos los jugadores de la Lobby en formato LobbyPlayerData.
        /// </summary>
        /// <returns>El LobbyPlayerData con los datos de todos los jugadores.</returns>
        public List<LobbyPlayerData> GetAllPlayersDataObject()
        {
            List<LobbyPlayerData> players = new List<LobbyPlayerData>();
            List<Dictionary<string, PlayerDataObject>> playersData = GetAllPlayersData();
            playersData.ForEach(playerData => players.Add(new LobbyPlayerData(playerData)));
            return players;
        }

        /// <summary>
        /// Obtiene los datos de un jugador de la lobby en formato string y PlayerDataObject.
        /// </summary>
        /// <param name="playerId">El id del usuario a buscar</param>
        /// <returns>El diccionario con los datos del jugador</returns>
        public Dictionary<string, PlayerDataObject> GetSinglePlayerData(string playerId)
        {
            return LobbyDataManager.Instance.Lobby?.Players.FirstOrDefault(player => player.Id == playerId)?.Data;  
        }

        /// <summary>
        /// Obtiene los datos de un jugador de la lobby en formato LobbyPlayerData.
        /// </summary>
        /// <param name="playerId">El id del usuario a buscar.</param>
        /// <returns>El LobbyPlayerData con los datos del jugador.</returns>
        public LobbyPlayerData GetSinglePlayerDataObject(string playerId)
        {
            Dictionary<string, PlayerDataObject> playerData = GetSinglePlayerData(playerId);
            return new LobbyPlayerData(playerData);
        }

        /// <summary>
        /// Obtiene el equipo en la Lobby del jugador seleccionado.
        /// </summary>
        /// <param name="playerId">El id del usuario a buscar.</param>
        /// <returns>Enum PlayerTeam del jugador.</returns>
        public PlayerTeam GetPlayerTeam (string playerId)
        {
            LobbyPlayerData playerData = GetSinglePlayerDataObject(playerId);
            return playerData.PlayerTeam;
        }

        /// <summary>
        /// Declara la disponibilidad para jugar del jugador local en la Lobby.
        /// </summary>
        /// <param name="ready">True si el jugador está listo.</param>
        /// <returns>True si se completa el cambio correctamente.</returns>
        public async Task<bool> SetPlayerReadyAsync(bool ready)
        {
            return await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                LobbyPlayerData localPlayerData = GetSinglePlayerDataObject(UnityServicesActions.GetCurrentUserID());
                localPlayerData.IsReady = ready;
                return await UpdatePlayerData(localPlayerData.Id, localPlayerData.SerializeObjectToDictionary());
            });
        }

        /// <summary>
        /// Establece un nuevo equipo para el jugador seleccionado.
        /// </summary>
        /// <param name="playerData">Los datos actuales del jugador como LobbyPlayerData.</param>
        /// <param name="playerTeam">Enum PlayerTeam a establecer.</param>
        /// <returns>True si se completa el cambio correctamente.</returns>
        public async Task<bool> SetPlayerTeamAsync(LobbyPlayerData playerData, PlayerTeam playerTeam)
        {
            playerData.PlayerTeam = playerTeam;
            return await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                return await UpdatePlayerData(playerData.Id, playerData.SerializeObjectToDictionary());
            });
        }

        /// <summary>
        /// Actualiza los datos del jugador local en el Lobby cuando se une a un partido mediante Relay.
        /// Incluye opcionalmente los datos de conexión de Unity Relay si se proporcionan.
        /// </summary>
        /// <param name="allocationId">
        /// (Opcional) Allocation ID asignado por Unity Relay para el host de la partida.
        /// Este valor solo debe proporcionarse por los clientes que se conectan a una sesión existente.
        /// </param>
        /// <param name="connectionData">
        /// (Opcional) Información de conexión codificada del cliente generada al unirse a la sesión Relay.
        /// Este valor solo debe proporcionarse por los clientes que se conectan a una sesión existente.
        /// </param>
        /// <returns>True si los datos del jugador se actualizan correctamente.</returns>
        public async Task<bool> UpdatePlayerData(string playerId, Dictionary<string, string> playerData, string allocationId = default, string connectionData = default)
        {
            return await SafeAsyncFunctionsHandler.ExecuteAsync( async () =>
            {
                UpdatePlayerOptions options = CreateUpdatePlayerOptions(playerData, allocationId, connectionData);
                return await InternalUpdatePlayer(playerId, options);
            });
        }

        /// <summary>
        /// Actualiza las opciones del jugador en la Lobby y los datos de union a Relay.
        /// Utilizar sólo cuando haya una conexión Relay disponible.
        /// </summary>
        /// <param name="playerId">El id del jugador</param>
        /// <param name="allocationId">Allocation ID asignado por Unity Relay para el host de la partida.</param>
        /// <param name="connectionData">Información de conexión codificada del cliente generada al unirse a la sesión Relay</param>
        /// <returns>True si se completa la actualización correctamente</returns>
        public async Task<bool> UpdatePlayerOptions(string playerId, string allocationId, string connectionData)
        {
            return await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                Dictionary<string, string> dictionaryPlayerData = GetSinglePlayerDataObject(playerId).SerializeObjectToDictionary();
                UpdatePlayerOptions options = CreateUpdatePlayerOptions(dictionaryPlayerData, allocationId, connectionData);
                return await InternalUpdatePlayer(playerId, options);
            }, false);
        }

        private async Task<bool> InternalUpdatePlayer(string playerId, UpdatePlayerOptions options)
        {
            return await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                await LobbyService.Instance.UpdatePlayerAsync(LobbyDataManager.Instance.GetLobbyID(), playerId, options);
                LobbyEvents.OnLobbyUpdated(LobbyDataManager.Instance.Lobby);
                return true;
            }, false);
        }

        private UpdatePlayerOptions CreateUpdatePlayerOptions(Dictionary<string, string> playerData, string allocationId, string connectionData)
        {
            return new UpdatePlayerOptions
            {
                Data = DataUtil.ToPlayerDataObjectDictionary(playerData),
                AllocationId = allocationId,
                ConnectionInfo = connectionData,
            };
        }
    }
}