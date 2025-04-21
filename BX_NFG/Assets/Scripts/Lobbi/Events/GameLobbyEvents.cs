
namespace Assets.Scripts.Lobbi
{
    public static class GameLobbyEvents
    {
        // This event is triggered when the lobby is updated (Only advertise)
        public delegate void LobbyUpdated();
        public static LobbyUpdated OnLobbyUpdated;
    }
}
