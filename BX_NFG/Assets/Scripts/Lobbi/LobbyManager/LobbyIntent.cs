using Assets.Scripts.Commons;

namespace Assets.Scripts.Lobbi.LobbyManager
{
    public class LobbyIntent : Singleton<LobbyIntent>
    {
        public bool IsCreatingLobby { get; set; }
        public string JoinCode { get; set; }
    }
}
