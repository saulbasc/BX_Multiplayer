
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi
{
    public static class LobbyEvents
    {
        public delegate void LobbyUpdate(Lobby lobby);
        public static LobbyUpdate OnLobbyUpdated;
    }
}
