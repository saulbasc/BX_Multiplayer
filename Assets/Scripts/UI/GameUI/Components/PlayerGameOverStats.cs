using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.GameUI.Components
{
    public class PlayerGameOverStats : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI playerGoalsText;
        [SerializeField] private TextMeshProUGUI playerTouchesText;

        public void SetPanelData(string playerName, int goals, int touches)
        {
            playerNameText.text = playerName;
            playerGoalsText.text = goals.ToString();
            playerTouchesText.text = touches.ToString();
        }
    }
}
