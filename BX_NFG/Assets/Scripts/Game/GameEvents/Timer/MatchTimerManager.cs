using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.GameManager.GameEvents.Timer
{
    public class MatchTimerManager : NetworkBehaviour
    {
        [SerializeField] private MatchStateManager matchStateManager;
        [SerializeField] private MatchDuration matchDuration;
        private bool isRunning = true;

        private NetworkVariable<float> timeRemaining = new NetworkVariable<float>(
            writePerm: NetworkVariableWritePermission.Server);

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                TimeAsignment();
                matchStateManager.OnMatchStateChanged += HandleStateChanged;
            }
        }

        private void OnDisable()
        {
            if (IsServer && matchStateManager != null)
            {
                matchStateManager.OnMatchStateChanged -= HandleStateChanged;
            }
        }

        private void Update()
        {
            if (!IsServer) return;

            if (isRunning)
            {
                PlayingTimer();
            }
            
            if (timeRemaining.Value <= 0)
            {
                StopTimer();
                matchStateManager.SetMatchState(MatchState.gameOver);
            }
        }

        private void HandleStateChanged(MatchState state)
        {
            if (!IsServer) return;

            if (state == MatchState.playing)
            {
                StartTimer();
            }
            else
            {
                StopTimer();
            }
        }

        private void StartTimer() => isRunning = true;
        private void StopTimer() => isRunning = false;

        private void TimeAsignment()
        {
            switch (matchDuration)
            {
                case MatchDuration.matchDuration1: timeRemaining.Value = 20f; break;
                case MatchDuration.matchDuration3: timeRemaining.Value = 180f; break;
                case MatchDuration.matchDuration5: timeRemaining.Value = 300f; break;
                case MatchDuration.matchDuration7: timeRemaining.Value = 420f; break;
                case MatchDuration.matchDuration10: timeRemaining.Value = 600f; break;
            }
        }

        private void PlayingTimer()
        {
            timeRemaining.Value -= Time.deltaTime;
        }

        public float GetTime()
        {
            return timeRemaining.Value;
        }
    }
}
