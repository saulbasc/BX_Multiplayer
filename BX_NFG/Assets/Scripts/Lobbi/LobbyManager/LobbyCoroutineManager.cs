using System;
using System.Collections;
using Assets.Scripts.Commons;
using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Corroutine;
using Unity.Services.Lobbies;
using UnityEngine;

namespace Assets.Scripts.Lobbi.Logic
{
    /// <summary>
    /// Permite gestionar la corrutina que matiene la sala activa.
    /// </summary>
    public class LobbyCoroutineManager : Singleton<LobbyCoroutineManager>
    {
        /// <summary>
        /// Crea una nueva corrutina para matener activa la Lobby y la guarda en el CoroutineManager.
        /// </summary>
        /// <param name="lobbyId">El identificador de la sala</param>
        /// <param name="interval">El intervalo en el que se ejecutará la corrutina</param>
        public void StartHeartbeatCororutine(string lobbyId, float interval)
        {
            CoroutineManager.Instance.StartTrackedCoroutine(
                CoroutineIndentifier.LobbyHeartbeatCoroutine, 
                HeartbeatCoroutine(lobbyId, interval)
            );
        }

        /// <summary>
        /// Crea una nueva corrutina para actualizar la Lobby y la guarda en el CoroutineManager.
        /// </summary>
        /// <param name="lobbyId">El identificador de la sala</param>
        /// <param name="interval">El intervalo en el que se ejecutará la corrutina</param>
        public void StartUpdateLobbyCororutine(string lobbyId, float interval)
        {
            CoroutineManager.Instance.StartTrackedCoroutine(
                CoroutineIndentifier.LobbyUpdateCoroutine,
                UpdateLobbyCoroutine(lobbyId, interval)
            );
        }

        /// <summary>
        /// Corrutina que mantiene la Lobby activa.
        /// </summary>
        /// <param name="lobbyId">El identificador de la sala.</param>
        /// <param name="wait">El intervalo en el que se ejecutará la corrutina.</param>
        /// <returns>La corrutina definida.</returns>
        private IEnumerator HeartbeatCoroutine(string lobbyId, float wait)
        {
            while (true)
            {
                try
                {
                    LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                yield return new WaitForSecondsRealtime(wait);
            }
        }

        /// <summary>
        /// Corrutina que actualiza la Lobby.
        /// </summary>
        /// <param name="lobbyId">El identificador de la sala.</param>
        /// <param name="wait">El intervalo en el que se ejecutará la corrutina.</param>
        /// <returns>La corrutina definida.</returns>
        private IEnumerator UpdateLobbyCoroutine(string lobbyId, float wait)
        {
            while (true)
            {
                var lobbyTask = LobbyService.Instance.GetLobbyAsync(lobbyId);
                yield return new WaitUntil(() => lobbyTask.IsCompleted);

                if (lobbyTask.IsCompletedSuccessfully)
                {
                    var latestLobby = lobbyTask.Result;
                    if (latestLobby.LastUpdated > LobbyDataManager.Instance.Lobby.LastUpdated)
                    {
                        LobbyEvents.Instance.RaiseNewLobbyUpdated(latestLobby);
                    }
                }
                else
                {
                    Debug.LogError($"Error al obtener el lobby: {lobbyTask.Exception?.Flatten().InnerException}");
                }

                yield return new WaitForSecondsRealtime(wait);
            }
        }

        /// <summary>
        /// Elimina la instancia de la clase como gameObject.
        /// </summary>
        public void Delete()
        {
            StopHeartbeatCoroutine();
            Destroy(gameObject);
        }

        /// <summary>
        /// Detiene la corrutina que mantiene la sala activa.
        /// </summary>
        private void StopHeartbeatCoroutine()
        {
            CoroutineManager.Instance.StopTrackedCoroutine(CoroutineIndentifier.LobbyHeartbeatCoroutine);
        }
    }
}
