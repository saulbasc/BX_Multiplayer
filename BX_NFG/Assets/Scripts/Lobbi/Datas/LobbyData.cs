
using System.Collections.Generic;
using Assets.Scripts.Lobbi.Data;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Assets.Scripts.Lobbi.Datas
{
    public class LobbyData
    {
        private string relayJoinCode;
        private string sceneName;

        public string RelayJoinCode
        {
            get { return relayJoinCode; }
            set { relayJoinCode = value; }
        }

        public string SceneName
        {
            get { return sceneName; }
            set { sceneName = value; }
        }

        public void Inizialice(string relayJoinCode, string sceneName)
        {
            this.relayJoinCode = relayJoinCode;
            this.sceneName = sceneName;
        }

        public void Inizialice(Dictionary<string, DataObject> lobbyData)
        {
            UpdateState(lobbyData);
        }

        private void UpdateState(Dictionary<string, DataObject> lobbyData)
        {
            if (lobbyData == null) 
            {
                Debug.Log("Lobby data is null");
                return;
            }
            if (lobbyData.ContainsKey(LobbyDataKeys.JoinRelayCode))
            {
                relayJoinCode = lobbyData[LobbyDataKeys.JoinRelayCode].Value?.ToString();
            }
            if(lobbyData.ContainsKey(LobbyDataKeys.SceneName))
            {
                sceneName = lobbyData[LobbyDataKeys.SceneName].Value?.ToString();
            }
        }

        public Dictionary<string, string> Serialize()
        {
            return new Dictionary<string, string>
            {
                { LobbyDataKeys.JoinRelayCode, relayJoinCode },
                { LobbyDataKeys.SceneName, sceneName },
            };
        }
    }
}
