
using System;
using System.Collections.Generic;
using Assets.Scripts.Lobbi.Data;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi
{
    public class LobbyPlayerData
    {
        private string id;
        private string gameTag;
        private bool isReady;
        private PlayerTeam playerTeam;

        public string Id => id;
        public string GameTag => gameTag;
        public bool IsReady
        {
            set => isReady = value;
            get => isReady;
        }

        public PlayerTeam PlayerTeam
        {
            set => playerTeam = value;
            get => playerTeam;
        }

        public void Inizialize(string id, string gameTag)
        {
            this.id = id;
            this.gameTag = gameTag;
            isReady = false;
            playerTeam = PlayerTeam.Spectator;
        }

        public void Inizialice(Dictionary<string, PlayerDataObject> playerData)
        {
            UpdateState(playerData);
        }

        private void UpdateState(Dictionary<string, PlayerDataObject> playerData)
        {
            if(playerData.ContainsKey(PlayerDataKeys.Id))
            {
                id = playerData[PlayerDataKeys.Id].Value;
            }
            if (playerData.ContainsKey(PlayerDataKeys.GameTag))
            {
                gameTag = playerData[PlayerDataKeys.GameTag].Value;
            }
            if (playerData.ContainsKey(PlayerDataKeys.IsReady))
            {
                isReady = playerData[PlayerDataKeys.IsReady].Value == "True";
            }
            if(playerData.ContainsKey(PlayerDataKeys.PlayerTeam))
            {
                string team = playerData[PlayerDataKeys.PlayerTeam].Value;
                if (Enum.TryParse(team, ignoreCase: true, out PlayerTeam parsedTeam))
                {
                    playerTeam = parsedTeam;
                }
            }
        }

        public Dictionary<string, string> Serialize()
        {
            return new Dictionary<string, string>
            {
                { PlayerDataKeys.Id, id },
                { PlayerDataKeys.GameTag, gameTag },
                { PlayerDataKeys.IsReady, isReady.ToString() },
                { PlayerDataKeys.PlayerTeam, playerTeam.ToString() },
            };
        }
    }
}
