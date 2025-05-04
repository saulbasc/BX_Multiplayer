
using System;
using System.Collections.Generic;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Assets.Scripts.Lobbi.Data;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Assets.Scripts.Lobbi.Datas
{
    public class LobbyData
    {
        private string relayJoinCode;
        private MatchDuration matchDuration;

        public string RelayJoinCode
        {
            get { return relayJoinCode; }
            set { relayJoinCode = value; }
        }

        public MatchDuration MatchDuration
        {
            get { return matchDuration; }
            set { matchDuration = value; }
        }

        public LobbyData(string relayJoinCode, MatchDuration matchDuration)
        {
            this.relayJoinCode = relayJoinCode;
            this.matchDuration = matchDuration;
        }

        public LobbyData(Dictionary<string, DataObject> lobbyData)
        {
            UpdateState(lobbyData);
        }

        public LobbyData() { }

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
            if (lobbyData.ContainsKey(LobbyDataKeys.MatchDuration))
            {
                string matchDurationString = lobbyData[LobbyDataKeys.MatchDuration].Value?.ToString();
                if (Enum.TryParse(matchDurationString, out MatchDuration parsedMatchDuration))
                {
                    matchDuration = parsedMatchDuration;
                }
                else
                {
                    Debug.LogWarning($"Failed to parse MatchDuration from string: {matchDurationString}");
                }
            }
        }

        public Dictionary<string, string> Serialize()
        {
            return new Dictionary<string, string>
            {
                { LobbyDataKeys.JoinRelayCode, relayJoinCode },
                { LobbyDataKeys.MatchDuration, matchDuration.ToString() }
            };
        }
    }
}
