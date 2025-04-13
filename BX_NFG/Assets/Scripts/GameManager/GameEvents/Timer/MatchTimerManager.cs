using Assets.Scripts.GameManager.GameEvents.State;
using UnityEngine;

namespace Assets.Scripts.GameManager.GameEvents.Timer
{
    public class MatchTimerManager : MonoBehaviour
    {
        private MatchDuration matchDuration;
        private float timeRemaining;

        private bool isTimerCreated = false;

        public void Init( MatchDuration matchDuration )
        {
            this.matchDuration = matchDuration;
            TimeAsignment();
            isTimerCreated = true;
        }

        private void TimeAsignment()
        {
            if (matchDuration == MatchDuration.matchDuration1) timeRemaining = 60f;
            if (matchDuration == MatchDuration.matchDuration3) timeRemaining = 180f;
            if (matchDuration == MatchDuration.matchDuration5) timeRemaining = 300f;
            if (matchDuration == MatchDuration.matchDuration7) timeRemaining = 420f;
            if (matchDuration == MatchDuration.matchDuration10) timeRemaining = 600f;
        }

        void Update()
        {
            if(!isTimerCreated) return;
        }

        private void PlayingTimer()
        {
            timeRemaining -= Time.deltaTime;
        }
    }
}
