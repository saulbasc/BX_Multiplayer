using Assets.Scripts.Commons;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.GameManager.GameEvents.Timer
{
    public class MatchTimerManager : NetworkSingleton<MatchTimerManager>
    {
        [SerializeField] private MatchStateManager matchStateManager;
        private MatchDuration matchDuration;
        private bool isRunning = false;

        private NetworkVariable<float> timeRemaining = new NetworkVariable<float>(
            writePerm: NetworkVariableWritePermission.Server);

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                SetMatchDuration();
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

            if (!isRunning) return;

            PlayingTimer();
            
            if (timeRemaining.Value <= 0f)
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

        private void PlayingTimer()
        {
            timeRemaining.Value -= Time.deltaTime;
        }

        private void SetMatchDuration()
        {
            timeRemaining.Value = MatchInfo.Instance.Match.MatchDuration;
            Debug.Log("Match duration in sec => " + timeRemaining.Value);
        }

        public float GetTime() => timeRemaining.Value;
    }
}
