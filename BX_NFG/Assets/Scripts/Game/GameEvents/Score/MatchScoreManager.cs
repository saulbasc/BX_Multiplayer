
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
            MatchInfo.Instance.AddLocalGoal();
            ScoreEvents.OnUpdateLocalGoalScored?.Invoke(MatchInfo.Instance.GetLocalScore());
            matchStateManager.SetMatchState(MatchState.onGoal);
        }

        private void OnVisitorGoalScored()
        {
            MatchInfo.Instance.AddVisitorGoal();
            ScoreEvents.OnUpdateVisitorGoalScored?.Invoke(MatchInfo.Instance.GetVisitorScore());
            matchStateManager.SetMatchState(MatchState.onGoal);
        }
    }
}
