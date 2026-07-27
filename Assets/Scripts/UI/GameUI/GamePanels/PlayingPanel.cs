using Assets.Scripts.GameManager.GameEvents.State;
using UnityEngine;

namespace Assets.Scripts.UI.GameUI.GamePanels
{
    public class PlayingPanel : MonoBehaviour
    {
        [SerializeField] private MatchStateManager MatchStateManager;
        private void Awake()
        {
            MatchStateManager.OnMatchStateChanged += OnMatchStateChanged;
        }

        private void OnDestroy()
        {
            if (MatchStateManager != null)
            {
                MatchStateManager.OnMatchStateChanged -= OnMatchStateChanged;
            }
        }

        private void OnMatchStateChanged(MatchState state)
        {
            if (state == MatchState.gameOver)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
