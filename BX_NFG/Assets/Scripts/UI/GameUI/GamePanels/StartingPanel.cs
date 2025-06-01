using System;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.GameManager.GameEvents.State;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.GameUI.GamePanels
{
    public class StartingPanel : MonoBehaviour
    {
        [SerializeField] private GameObject startingPanel;
        [SerializeField] private TextMeshProUGUI startingText;

        private void Awake()
        {
            MatchStateManager.Instance.OnMatchStateChanged += HandleMatchStateChanged;
            GameStatusManager.OnUpdateSecondsLeft += UpdateStartingText;
            startingText.text = "Empezando...";
        }

        private void UpdateStartingText(int secondsLeft)
        {
            startingText.text = secondsLeft.ToString();
        }

        private void HandleMatchStateChanged(MatchState state)
        {
            if(state == MatchState.preMatch)
            {
                startingPanel.SetActive(true);
                startingText.text = "Empezando...";
            }
            else
            {
                startingText.text = "";
            }
        }
    }
}
