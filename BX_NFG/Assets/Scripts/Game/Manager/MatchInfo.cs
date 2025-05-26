using Assets.Scripts.Commons;
using Assets.Scripts.Core.Models;
using Assets.Scripts.GameManager.GameEvents.Timer;
using UnityEngine;

namespace Assets.Scripts.Game.Manager
{
    public class MatchInfo : NetworkSingleton<MatchInfo>
    {
        public int NumberOfPlayersInTeams { get; private set; }
        public int NumberOfPlayersInTeamsConnected { get; private set; }
        public Match Match { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void SetMatchDuration(MatchDuration matchDuration)
        {
            Match = new Match(MatchDurationExtensions.ToInt(matchDuration));
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
    }
}
