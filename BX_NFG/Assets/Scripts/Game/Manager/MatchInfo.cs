using System.Threading;
using Assets.Scripts.Commons;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.Manager
{
    public class MatchInfo : Singleton<MatchInfo>
    {
        public MatchDuration MatchDuration { get; set; }
        public int NumberOfPlayersInTeams { get; set; }
        public int NumberOfPlayersInTeamsConnected { get; private set; }

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
