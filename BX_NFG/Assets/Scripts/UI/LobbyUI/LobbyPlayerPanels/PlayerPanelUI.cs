using TMPro;
using UnityEngine;

namespace Assets.Scripts.Lobbi.Players
{
    public class PlayerPanelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;

        public void SetPlayerName(string playerName, bool isReady)
        {
            if(playerNameText != null)
            {
                playerNameText.text = playerName;
                playerNameText.color = isReady ? Color.green : Color.red;
            }
        }
    }
}
