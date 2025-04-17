using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Commons;

namespace Assets.Scripts.Connection
{
    public class GameLobbyManager : Singleton<GameLobbyManager>
    {
        public async Task<bool> CreateLobby()
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "GameTag", "HostPlayer" },
            };
            bool success = await LobbyManager.Instance.CreateLobby(4, false, data);
            return success;
        }

        public string GetLobbyCode()
        {
            return LobbyManager.Instance.GetLobbyCode();
        }

        public async Task<bool> JoinLobby(string code)
        {
            Dictionary<string, string> playerData = new Dictionary<string, string>
            {
                { "GameTag", "JoinPlayer" },
            };

            bool success = await LobbyManager.Instance.JoinLobby(code, playerData);
            return success;
        }
    }
}
