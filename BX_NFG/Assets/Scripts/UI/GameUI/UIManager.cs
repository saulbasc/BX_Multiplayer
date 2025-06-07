using System;
using System.Collections;
using Assets.Scripts.Commons;
using Assets.Scripts.Game.GameEvents.Score;
using Assets.Scripts.GameManager.GameEvents.State;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Assets.Scripts.UI.LobbyUI;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.GameManager.GameEvents.UI
{
    public class UIManager : NetworkBehaviour
    {
        private LobbyActionsManager lobbyActionsManager;

        [SerializeField] private MatchStateManager matchStateManager;
        [SerializeField] private MatchTimerManager timerManager;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI localGoalsText;
        [SerializeField] private TextMeshProUGUI visitorGoalsText;
        [SerializeField] private TextMeshProUGUI spectatorTimerText;
        [SerializeField] private TextMeshProUGUI spectatorLocalGoalsText;
        [SerializeField] private TextMeshProUGUI spectatorVisitorGoalsText;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button spectatorExitButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button resumeButton;

        private void Start()
        {
            if(pauseButton != null)
            {
                pauseButton.onClick.AddListener(OnPauseButtonClicked);
                resumeButton.onClick.AddListener(OnResumeButtonClicked);
                exitButton.onClick.AddListener(OnExitButtonClicked);
                spectatorExitButton.onClick.AddListener(OnExitButtonClicked);
            }
        }

        public override void OnNetworkSpawn()
        {
            ScoreEvents.OnUpdateLocalGoalScored += OnUpdateLocalGoalScoredRpc;
            ScoreEvents.OnUpdateVisitorGoalScored += OnUpdateVisitorGoalsScoredRpc;
            matchStateManager.OnMatchStateChanged += HandleStateChanged;
            StartCoroutine(SetUIManager());
        }

        private void HandleStateChanged(MatchState state)
        {
            if (state == MatchState.pause)
            {
                SetPauseButtons(true);
            }
            else
            {
                SetPauseButtons(false);
            }
        }

        private IEnumerator SetUIManager()
        {
            while (lobbyActionsManager == null)
            {
                lobbyActionsManager = FindAnyObjectByType<LobbyActionsManager>();
                yield return null;
            }
        }

        public override void OnNetworkDespawn()
        {
            ScoreEvents.OnUpdateLocalGoalScored -= OnUpdateLocalGoalScoredRpc;
            ScoreEvents.OnUpdateVisitorGoalScored -= OnUpdateVisitorGoalsScoredRpc;
            matchStateManager.OnMatchStateChanged -= HandleStateChanged;
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void OnUpdateLocalGoalScoredRpc(int goals)
        {
            localGoalsText.text = goals.ToString();
            spectatorLocalGoalsText.text = goals.ToString();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void OnUpdateVisitorGoalsScoredRpc(int goals)
        {
            visitorGoalsText.text = goals.ToString();
            spectatorVisitorGoalsText.text = goals.ToString();
        }

        private void Update()
        {
            if (!IsClient) return;

            float time = timerManager.GetTime();
            timerText.text = FormatTime(time);
            spectatorTimerText.text = FormatTime(time);
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

        private void SetPauseButtons(bool paused)
        {
            if (paused)
            {
                pauseButton.gameObject.SetActive(false);
                resumeButton.gameObject.SetActive(true);
            }
            else
            {
                resumeButton.gameObject.SetActive(false);
                pauseButton.gameObject.SetActive(true);
            }
        }

        private void OnResumeButtonClicked()
        {
            RequestResumeServerRpc();
        }

        private async void OnExitButtonClicked()
        {
            await lobbyActionsManager.ExitLobby();
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene(Scenes.MenuScene.ToString());
        }

        [Rpc(SendTo.Server)]
        private void RequestPauseServerRpc(RpcParams rpcParams = default)
        {
            matchStateManager.SetMatchState(MatchState.pause);
        }

        [Rpc(SendTo.Server)]
        private void RequestResumeServerRpc(RpcParams rpcParams = default)
        {
            matchStateManager.SetMatchState(MatchState.playing);
        }
    }
}
