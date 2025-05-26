using Assets.Scripts.Commons;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.GameUI.GamePanels
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button exitButton;

        private void Awake()
        {
            MatchStateManager.Instance.OnMatchStateChanged += OnMatchStateChanged;
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
            SceneManager.LoadScene(Scenes.MenuScene.ToString());
        }
    }
}
