using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game.GameEvents.Player;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.Manager
{
    public class MatchInfo : NetworkBehaviour
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
            localScore.Value++;
        }

        public void AddVisitorGoal()
        {
            visitorScore.Value++;
        }

        public void SetMatchDuration(MatchDuration matchDuration)
        {
            this.matchDuration.Value = MatchDurationExtensions.ToFloat(matchDuration);
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
            Debug.Log("NumberOfPlayersInTeams set to: " + NumberOfPlayersInTeams);
        }
        
        public void SetNumberOdPlayersInTeamsConnected(int numberOfPlayers)
        {
            NumberOfPlayersInTeamsConnected = numberOfPlayers;
            Debug.Log("NumberOfPlayersInTeamsConnected set to: " + NumberOfPlayersInTeamsConnected);
        }

        public bool GetAllConnected()
        {
            Debug.Log("NumberOfPlayersInTeamsConnected: " + NumberOfPlayersInTeamsConnected);
            Debug.Log("NumberOfPlayersInTeams: " + NumberOfPlayersInTeams);
            return NumberOfPlayersInTeamsConnected == NumberOfPlayersInTeams;
        }

        public List<PlayerInGame> GetPlayersInGame()
        {
            return FindObjectsByType<PlayerInGame>(FindObjectsSortMode.None).ToList();
        }
    }
}
