using System;
using Assets.Scripts.Commons;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi
{
    public class LobbyEvents : DefaultSingleton<LobbyEvents>
    {
        public event Action<Lobby> OnNewLobbyUpdated;
        public event Action OnLobbyUpdated;
        public event Action OnLobbyReady;
        public event Action OnLobbyCancel;

        /// <summary>
        /// Evento que manda la nueva Lobby actualizada.
        /// </summary>
        /// <param name="updatedLobby">La nueva Lobby.</param>
        public void RaiseNewLobbyUpdated(Lobby updatedLobby) => OnNewLobbyUpdated?.Invoke(updatedLobby);
        /// <summary>
        /// Evento que avisa la actualización de la Lobby.
        /// </summary>
        public void RaiserLobbyUpdated() => OnLobbyUpdated?.Invoke();
        /// <summary>
        /// Evento que indica que la sala está lista para entrar al juego.
        /// </summary>
        public void RaiserLobbyReady() => OnLobbyReady?.Invoke();
        /// <summary>
        /// Evento que indica que la sala ya no está lista para entrar al juego.
        /// </summary>
        public void RaiserLobbyCancel() => OnLobbyCancel?.Invoke();
    }
}
