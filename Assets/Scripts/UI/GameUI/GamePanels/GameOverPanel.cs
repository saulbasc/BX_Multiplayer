using Assets.Scripts.Commons;
using Assets.Scripts.GameManager.GameEvents.State;
using Assets.Scripts.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.GameUI.GamePanels
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private MatchStateManager matchStateManager;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            matchStateManager.OnMatchStateChanged += OnMatchStateChanged;
        }

        private void OnDestroy()
        {
            if (matchStateManager != null)
            {
                matchStateManager.OnMatchStateChanged -= OnMatchStateChanged;
            }
        }

        private void OnMatchStateChanged(MatchState state)
        {
            if(state == MatchState.gameOver)
            {
                gameOverPanel.SetActive(true);
                exitButton.onClick.AddListener(OnExitButtonClicked);
            }
            else
            {
                exitButton.onClick.RemoveListener(OnExitButtonClicked);
                gameOverPanel.SetActive(false);
            }
        }

        private void OnExitButtonClicked()
        {
            SoundEvents.Instance.RaiseEndMatchSound();
            matchStateManager.SetMatchState(MatchState.exit);
            SceneManager.LoadScene(Scenes.MenuScene.ToString());
        }
    }
}
