
using Assets.Scripts.Commons;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.Manager
{
    public class MatchInfo : Singleton<MatchInfo>
    {
        public MatchDuration MatchDuration { get; private set; }
        public void SetMatchDuration(MatchDuration matchDuration)
        {
            MatchDuration = matchDuration;
        }

        public int NumberOfPlayersInTeams { get; private set; }
        public void SetNumberOfPlayersInTeams(int numberOfPlayers)
        {
            NumberOfPlayersInTeams = numberOfPlayers;
        }

        public int NumberOfPlayersInTeamsConnected { get; private set; }
        public void SetNumberOdPlayersInTeamsConnected(int numberOfPlayers)
        {
            NumberOfPlayersInTeamsConnected = numberOfPlayers;
        }

        [Rpc(SendTo.Server)]
        public void AddNewPlayerConnectedServerRpc()
        {
            NumberOfPlayersInTeamsConnected++;
            Debug.Log("Número de jugadores conectados actualizado a: " + NumberOfPlayersInTeamsConnected);
        }

        public bool GetAllConnected()
        {
            Debug.Log("NumberOfPlayersInTeamsConnected: " + NumberOfPlayersInTeamsConnected);
            Debug.Log("NumberOfPlayersInTeams: " + NumberOfPlayersInTeams);
            return NumberOfPlayersInTeamsConnected == NumberOfPlayersInTeams;
        }
    }
}
