
using Assets.Scripts.Game.GameEvents.Score;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.GameManager.GameEvents
{
    public class MatchScoreManager : NetworkBehaviour
    {
        [SerializeField] private MatchInfo matchInfo;
        [SerializeField] private MatchStateManager matchStateManager;
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
            matchInfo.AddLocalGoal();
            ScoreEvents.OnUpdateLocalGoalScored?.Invoke(matchInfo.GetLocalScore());
            matchStateManager.SetMatchState(MatchState.onGoal);
        }

        private void OnVisitorGoalScored()
        {
            matchInfo.AddVisitorGoal();
            ScoreEvents.OnUpdateVisitorGoalScored?.Invoke(matchInfo.GetVisitorScore());
            matchStateManager.SetMatchState(MatchState.onGoal);
        }
    }
}
