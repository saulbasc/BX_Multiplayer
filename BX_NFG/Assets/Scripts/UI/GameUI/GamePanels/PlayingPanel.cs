using Assets.Scripts.GameManager.GameEvents.State;
using UnityEngine;

namespace Assets.Scripts.UI.GameUI.GamePanels
{
    public class PlayingPanel : MonoBehaviour
    {
        private void Awake()
        {
            MatchStateManager.Instance.OnMatchStateChanged += OnMatchStateChanged;
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
