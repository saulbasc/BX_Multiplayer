using Unity.Services.Lobbies.Models;
using UnityEngine;
using Assets.Scripts.Relay;
using WebSocketSharp;

namespace Assets.Scripts.Lobbi.Logic
{
    /// <summary>
    /// Clase dedicada a la actualización periódica de la Lobby
    /// </summary>
    public class LobbyUpdaterManager : MonoBehaviour
    {
        [SerializeField] private ClientRelayManager clientRelayManager;
        [SerializeField] private LobbyDataManager lobbyDataManager;
        private void OnEnable()
        {
            LobbyEvents.Instance.OnNewLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            LobbyEvents.Instance.OnNewLobbyUpdated -= OnLobbyUpdated;
            Destroy(gameObject);
        }

        private void OnLobbyUpdated(Lobby lobby)
        {
            lobbyDataManager.SetLobby(lobby);

            LobbyEvents.Instance.RaiserLobbyUpdated();

            CheckIfNumberOfPlayersReadyInLobby();

            CheckIfLocalPlayerIsReadyToEnterInGame();
        }

        /// <summary>
        /// Comprueba si todos los jugadores de la Lobby están listos.
        /// Si todos los jugadores están listos lanza un evento de Lobby lista para jugar.
        /// </summary>
        private void CheckIfNumberOfPlayersReadyInLobby()
        {
            if (lobbyDataManager.NumberOfPlayersReady() == lobbyDataManager.GetNumberOfPlayers())
            {
                LobbyEvents.Instance.RaiserLobbyReady();
            }
            else
            {
                LobbyEvents.Instance.RaiserLobbyCancel();
            }
        }

        /// <summary>
        /// Comprueba si el jugador es apto para entrar en la escena del juego.
        /// Si es apto se une al RelayServer y entra en la escena del juego.
        /// </summary>
        private async void CheckIfLocalPlayerIsReadyToEnterInGame()
        {
            string joinRelayCode = lobbyDataManager.GetLobbyDataObject().RelayJoinCode;

            if (joinRelayCode.IsNullOrEmpty() || PlayerStatus.Instance.InGame || PlayerStatus.Instance.JoinedGame) return;

            LobbyEvents.Instance.RaiserLobbyStart();
            await clientRelayManager.JoinRelayServer();
            PlayerStatus.Instance.JoinedGame = true;
        }

        /// <summary>
        /// Elimina la instancia gameObject de ka clase.
        /// </summary>
        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}
