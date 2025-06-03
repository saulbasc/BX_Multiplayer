
using Assets.Scripts.Game.GameEvents.Score;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;

namespace Assets.Scripts.GameManager.GameEvents
{
    public class LocalScoreManager : NetworkBehaviour
    {
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
            MatchStateManager.Instance.SetMatchState(MatchState.onGoal);
        }

        private void OnVisitorGoalScored()
        {
            MatchInfo.Instance.AddVisitorGoal();
            ScoreEvents.OnUpdateVisitorGoalScored?.Invoke(MatchInfo.Instance.GetVisitorScore());
            MatchStateManager.Instance.SetMatchState(MatchState.onGoal);
        }
    }
}
