using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game.GameEvents.Player;
using Assets.Scripts.GameManager.GameEvents.State;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.Manager
{
    public class MatchInfo : NetworkBehaviour
    {
        [SerializeField] private MatchStateManager matchStateManager;

        public int NumberOfPlayersInTeams { get; private set; }
        public int NumberOfPlayersInTeamsConnected { get; private set; }

        public NetworkList<FinalPlayerStatsData> playerStats = new(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

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
            if (IsServer)
            {
                localScore.Value = 0;
                visitorScore.Value = 0;
                matchDuration.Value = 60;
                StartCoroutine(StartMatchInfo());
            }
        }

        private IEnumerator StartMatchInfo()
        {
            while(matchStateManager == null)
            {
                matchStateManager = FindAnyObjectByType<MatchStateManager>();
                yield return null;
            }
            matchStateManager.OnMatchStateChanged += OnMatchStateChanged;
        }

        private void OnMatchStateChanged(MatchState state)
        {
            if ( state == MatchState.gameOver)
            {
                foreach (var playerInGame in GetPlayersInGame())
                {
                    if (playerInGame.PlayerId != null)
                    {
                        RegisterPlayerStats(playerInGame);
                    }
                }
            }
        }

        private void RegisterPlayerStats(PlayerInGame player)
        {
            FinalPlayerStatsData data = new FinalPlayerStatsData
            {
                PlayerName = player?.TagName,
                Goals = player.Goals,
                Touches = player.Touches,
                PlayerTeam = player.Team
            };

            playerStats.Add(data);
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

        private List<PlayerInGame> GetPlayersInGame()
        {
            return FindObjectsByType<PlayerInGame>(FindObjectsSortMode.None).ToList();
        }
    }
}
