
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi
{
    public static class LobbyEvents
    {
        // This event is triggered when the lobby is updated and pass de lobby
        public delegate void LobbyUpdate(Lobby lobby);
        public static LobbyUpdate OnLobbyUpdated;
    }
}
