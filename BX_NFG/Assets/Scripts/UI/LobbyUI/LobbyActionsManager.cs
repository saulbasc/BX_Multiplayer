using System;
using System.Threading.Tasks;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Assets.Scripts.Lobbi.Logic;
using UnityEngine;

namespace Assets.Scripts.UI.LobbyUI
{
    /// <summary>
    /// Clase dedicada a las acciones que puede acometer el jugador en la Labby.
    /// </summary>
    public class LobbyActionsManager : MonoBehaviour
    {
        [SerializeField] private LobbyServiceManager lobbyServiceManager;
        [SerializeField] private LobbyPlayerManager LobbyPlayerManager;
        [SerializeField] private LobbyDataManager lobbyDataManager;
        [SerializeField] private LobbyCoroutineManager lobbyCoroutineManager;
        /// <summary>
        /// El jugador abandona la Lobby.
        /// </summary>
        /// <returns>True si abandona con éxito la Lobby.</returns>
        public async Task<bool> ExitLobby()
        {
            return await lobbyServiceManager.DisconnectFromLobby();
        }

        /// <summary>
        /// Establece si el jugador local está listo o no en la Lobby.
        /// </summary>
        /// <param name="isReady">True si el jugador está listo.</param>
        /// <returns>True si se establece el cambio correctamente.</returns>
        public async Task<bool> SetLocalLobbyPlayerReadyStatus(bool isReady)
        { 
            return await LobbyPlayerManager.SetPlayerReadyAsync(isReady);
        }

        /// <summary>
        /// /// Incrementa o decrementa la duración del partido en un valor y lo asigna a los datos de la Lobby.
        /// </summary>
        /// <param name="increase">True para aumentar la duración y false para decrementar.</param>
        /// <returns>True si se completa la operación completamente.</returns>
        public async Task<bool> ChangeMatchDuration(bool increase)
        {
            MatchDuration[] durations = MatchDurationExtensions.MatchDurationList();
            MatchDuration currentMatchDuration = lobbyDataManager.GetLobbyMatchDuration();

            int index = Array.IndexOf(durations, currentMatchDuration);
            int newIndex = increase ? index + 1 : index - 1;

            if (newIndex >= 0 && newIndex < durations.Length)
            {
                MatchDuration newMatchDuration = durations[newIndex];
                return await lobbyDataManager.SetLobbyMatchDurationAsync(newMatchDuration);
            }

            return false;
        }
    }
}
