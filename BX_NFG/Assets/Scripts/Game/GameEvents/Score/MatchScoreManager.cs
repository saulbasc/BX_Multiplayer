
using Assets.Scripts.Game.GameEvents.Score;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.GameManager.GameEvents
{
    public class LocalScoreManager : NetworkBehaviour
    {
        [SerializeField] private MatchStateManager matchStateManager;

        private int localScore;
        private int visitorScore;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                ScoreEvents.OnLocalGoalScored += OnLocalGoalScored;
                ScoreEvents.OnVisitorGoalScored += OnVisitorGoalScored;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                ScoreEvents.OnLocalGoalScored -= OnLocalGoalScored;
                ScoreEvents.OnVisitorGoalScored -= OnVisitorGoalScored;
            }
        }

        private void OnLocalGoalScored()
        {
            localScore++;
            MatchInfo.Instance.Match.LocalTeam.AddGoal();
            ScoreEvents.OnUpdateLocalGoalScored?.Invoke(localScore);
            matchStateManager.SetMatchState(MatchState.onGoal);
        }

        private void OnVisitorGoalScored()
        {
            visitorScore++;
            MatchInfo.Instance.Match.VisitorTeam.AddGoal();
            ScoreEvents.OnUpdateVisitorGoalScored?.Invoke(visitorScore);
            matchStateManager.SetMatchState(MatchState.onGoal);
        }
    }
}
