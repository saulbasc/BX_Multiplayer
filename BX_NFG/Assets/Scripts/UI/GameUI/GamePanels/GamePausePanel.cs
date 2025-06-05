using Assets.Scripts.GameManager.GameEvents.State;
using UnityEngine;

namespace Assets.Scripts.UI.GameUI.GamePanels
{
    public class GamePausePanel : MonoBehaviour
    {
        [SerializeField] private MatchStateManager matchStateManager;
        [SerializeField] private GameObject pausePanel;

        private void Awake()
        {
            matchStateManager.OnMatchStateChanged += OnMatchStateChanged;
            pausePanel.SetActive(false);
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
            if (state == MatchState.pause)
            {
                pausePanel.SetActive(true);
            }
            else
            {
                pausePanel.SetActive(false);
            }
        }
    }
}
