using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Commons;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Game.GameEvents.Player;
using Assets.Scripts.GameManager.GameEvents.Timer;
using NUnit.Framework;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.Manager
{
    public class MatchInfo : NetworkSingleton<MatchInfo>
    {
        public int NumberOfPlayersInTeams { get; private set; }
        public int NumberOfPlayersInTeamsConnected { get; private set; }

        private NetworkVariable<int> localScore = new();
        private NetworkVariable<int> visitorScore = new();
        private NetworkVariable<float> matchDuration = new();

        public int GetLocalScore() => localScore.Value;
        public int GetVisitorScore() => visitorScore.Value;
        public float GetMatchDuration() => matchDuration.Value;

        public void AddLocalGoal()
        {
            Debug.Log("GOAL AÑADIDO A MATCH INFO");
            localScore.Value++;
        }

        public void AddVisitorGoal()
        {
            Debug.Log("GOAL AÑADIDO A MATCH INFO");
            visitorScore.Value++;
        }

        public void SetMatchDuration(MatchDuration matchDuration)
        {
            this.matchDuration.Value = MatchDurationExtensions.ToFloat(matchDuration);
            Debug.Log("SEEEEEEEEEEEEEEEET MATCH DURATION");
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                localScore.Value = 0;
                visitorScore.Value = 0;
                matchDuration.Value = 50;
            }
        }
        
        public void SetNumberOfPlayersInTeams(int numberOfPlayers)
        {
            NumberOfPlayersInTeams = numberOfPlayers;
        }
        
        public void SetNumberOdPlayersInTeamsConnected(int numberOfPlayers)
        {
            NumberOfPlayersInTeamsConnected = numberOfPlayers;
        }

        public bool GetAllConnected()
        {
            Debug.Log("NumberOfPlayersInTeamsConnected: " + NumberOfPlayersInTeamsConnected);
            Debug.Log("NumberOfPlayersInTeams: " + NumberOfPlayersInTeams);
            return NumberOfPlayersInTeamsConnected == NumberOfPlayersInTeams;
        }

        public List<PlayerInGame> GetPlayersInGame()
        {
            Debug.Log("NO SE SI SOY SERVER");
            //if (!IsServer) return new List<PlayerInGame>();
            Debug.Log("SOY SERVER");

            return FindObjectsByType<PlayerInGame>(FindObjectsSortMode.None).ToList();
        }
    }
}
