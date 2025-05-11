
namespace Assets.Scripts.Lobbi
{
    public static class GameLobbyEvents
    {
        public delegate void LobbyUpdated();
        public static LobbyUpdated OnLobbyUpdated;

        public delegate void LobbyReady();
        public static LobbyReady OnLobbyReady;

        public delegate void LobbyCancel();
        public static LobbyCancel OnLobbyCancel;
    }
}
