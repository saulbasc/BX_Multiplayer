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
    public class LobbyHeartbeatManager : Singleton<LobbyHeartbeatManager>
    {
        /// <summary>
        /// Crea una nueva corrutina para matener activa la sala y la guarda en el CoroutineManager.
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
        /// Corrutina que mantiene la sala activa.
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
