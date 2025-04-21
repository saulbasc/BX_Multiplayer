
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Lobbi
{
    public class LobbyPlayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;

        private LobbyPlayerData playerData;

        public void SetPlayerData(LobbyPlayerData data)
        {
            playerData = data;
            playerName.text = playerData.GameTag;
            gameObject.SetActive(true);
            if (playerData.IsReady) { playerName.color = Color.green; }
        }
    }
}
