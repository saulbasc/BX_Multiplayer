using System.Collections;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.GameManager.GameEvents.Timer
{
    public class MatchTimerManager : NetworkBehaviour
    {
        [SerializeField] private MatchInfo matchInfo;
        [SerializeField] private MatchStateManager matchStateManager;
        private bool isRunning = false;
        private NetworkVariable<float> timeRemaining = new NetworkVariable<float>(
            writePerm: NetworkVariableWritePermission.Server);

        public override void OnNetworkSpawn()
        {
            Debug.Log("osiudhduogewyfewife");
            if (IsServer)
            {
                StartCoroutine(SetMatchDuration());
                matchStateManager.OnMatchStateChanged += HandleStateChanged;
            }
        }

        private void OnDisable()
        {
            if (IsServer)
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

        private IEnumerator SetMatchDuration()
        {
            yield return new WaitForSeconds(1f);
            timeRemaining.Value = matchInfo.GetMatchDuration();
        }

        public float GetTime() => timeRemaining.Value;
    }
}
