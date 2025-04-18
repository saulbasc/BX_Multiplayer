
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi
{
    public class LobbyPlayerData
    {
        private string id;
        private string gameTag;
        private bool isReady;

        public string Id => id;
        public string Name => gameTag;
        public bool IsReady
        {
            set => isReady = value;
            get => isReady;
        }

        public void Inizialize(string id, string name)
        {
            this.id = id;
            this.gameTag = name;
            isReady = false;
        }

        public void Inizialice(Dictionary<string, PlayerDataObject> playerData)
        {
            UpdateState(playerData);
        }

        private void UpdateState(Dictionary<string, PlayerDataObject> playerData)
        {
            if(playerData.ContainsKey("Id"))
            {
                id = playerData["Id"].Value;
            }
            if (playerData.ContainsKey("Name"))
            {
                gameTag = playerData["Name"].Value;
            }
            if (playerData.ContainsKey("IsReady"))
            {
                isReady = playerData["IsReady"].Value == "True";
            }
        }

        public Dictionary<string, string> Serialize()
        {
            return new Dictionary<string, string>
            {
                { "Id", id },
                { "Name", gameTag },
                { "IsReady", isReady.ToString() }
            };
        }
    }
}
