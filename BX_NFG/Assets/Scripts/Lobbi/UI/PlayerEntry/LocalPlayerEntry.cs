
using Assets.Scripts.Connection.Lobbi;
using Assets.Scripts.Lobbi.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Lobbi.UI.PlayerEntry
{
    public class LocalPlayerEntry : MonoBehaviour
    {
        [SerializeField] private Button rightButton;

        private void OnEnable()
        {
            rightButton.onClick.AddListener(OnRightButtonClick);
        }

        private void OnDisable()
        {
            rightButton.onClick.RemoveListener(OnRightButtonClick);
        }

        private async void OnRightButtonClick()
        {
            await GameLobbyManager.Instance.SetPlayerTeam(PlayerTeam.Spectator);
        }
    }
}
