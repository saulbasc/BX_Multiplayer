using Assets.Scripts.GameManager.GameEvents.State;
using UnityEngine;

namespace Assets.Scripts.UI.GameUI.GamePanels
{
    public class GamePausePanel : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;

        private void Awake()
        {
            MatchStateManager.Instance.OnMatchStateChanged += OnMatchStateChanged;
            pausePanel.SetActive(false);
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
