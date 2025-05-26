
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
            MatchInfo.Instance.Match.LocalTeam.AddGoal();
            ScoreEvents.OnUpdateLocalGoalScored?.Invoke(MatchInfo.Instance.Match.LocalTeam.Score);
            MatchStateManager.Instance.SetMatchState(MatchState.onGoal);
        }

        private void OnVisitorGoalScored()
        {
            MatchInfo.Instance.Match.VisitorTeam.AddGoal();
            ScoreEvents.OnUpdateVisitorGoalScored?.Invoke(MatchInfo.Instance.Match.VisitorTeam.Score);
            MatchStateManager.Instance.SetMatchState(MatchState.onGoal);
        }
    }
}
