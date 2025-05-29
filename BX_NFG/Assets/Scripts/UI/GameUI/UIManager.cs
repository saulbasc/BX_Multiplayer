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
        [SerializeField] private MatchTimerManager timerManager;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI localGoalsText;
        [SerializeField] private TextMeshProUGUI visitorGoalsText;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button resumeButton;

        private void Start()
        {
            if(pauseButton != null)
            {
                pauseButton.onClick.AddListener(OnPauseButtonClicked);
                resumeButton.onClick.AddListener(OnResumeButtonClicked);
                exitButton.onClick.AddListener(OnExitButtonClicked);
            }
        }

        public override void OnNetworkSpawn()
        {
            ScoreEvents.OnUpdateLocalGoalScored += OnUpdateLocalGoalScoredRpc;
            ScoreEvents.OnUpdateVisitorGoalScored += OnUpdateVisitorGoalsScoredRpc;
        }

        public override void OnNetworkDespawn()
        {
            ScoreEvents.OnUpdateLocalGoalScored -= OnUpdateLocalGoalScoredRpc;
            ScoreEvents.OnUpdateVisitorGoalScored -= OnUpdateVisitorGoalsScoredRpc;
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void OnUpdateLocalGoalScoredRpc(int goals)
        {
            localGoalsText.text = goals.ToString();
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void OnUpdateVisitorGoalsScoredRpc(int goals)
        {
            visitorGoalsText.text = goals.ToString();
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
            pauseButton.gameObject.SetActive(false);
            resumeButton.gameObject.SetActive(true);
        }

        private void OnResumeButtonClicked()
        {
            RequestResumeServerRpc();
            resumeButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
        }

        private async void OnExitButtonClicked()
        {
            NetworkManager.Singleton.Shutdown();
            await LobbyActionsManager.Instance.ExitLobby();
            SceneManager.LoadScene(Scenes.MenuScene.ToString());
        }

        [Rpc(SendTo.Server)]
        private void RequestPauseServerRpc(RpcParams rpcParams = default)
        {
            MatchStateManager.Instance.SetMatchState(MatchState.pause);
        }

        [Rpc(SendTo.Server)]
        private void RequestResumeServerRpc(RpcParams rpcParams = default)
        {
            MatchStateManager.Instance.SetMatchState(MatchState.playing);
        }
    }
}
