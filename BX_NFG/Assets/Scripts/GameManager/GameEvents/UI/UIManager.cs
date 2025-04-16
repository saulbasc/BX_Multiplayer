using System;
using Assets.Scripts.GameManager.GameEvents.State;
using Assets.Scripts.GameManager.GameEvents.Timer;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameManager.GameEvents.UI
{
    public class UIManager : NetworkBehaviour
    {
        [SerializeField] private MatchTimerManager timerManager;
        [SerializeField] private MatchStateManager matchStateManager;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Button pauseButton;

        private void Start()
        {
            if(pauseButton != null)
            {
                pauseButton.onClick.AddListener(OnPauseButtonClicked);
            }
        }

        private void Update()
        {
            if (!IsClient) return;

            float time = timerManager.GetTime();
            timerText.text = FormatTime(time);
        }

        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            return $"{minutes:00}:{seconds:00}";
        }

        private void OnPauseButtonClicked()
        {
            RequestPauseServerRpc();
        }

        [Rpc(SendTo.Server)]
        private void RequestPauseServerRpc(RpcParams rpcParams = default)
        {
            matchStateManager.SetMatchState(MatchState.pause);
        }
    }
}
