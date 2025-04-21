
using Assets.Scripts.Connection.Lobbi;
using Assets.Scripts.Lobbi.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Lobbi.UI
{
    public class SpectatorPlayerEntry : MonoBehaviour
    {
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;

        private void OnEnable()
        {
            leftButton.onClick.AddListener(OnLeftButtonClick);
            rightButton.onClick.AddListener(OnRightButtonClick);
        }

        private void OnDisable()
        {
            leftButton.onClick.RemoveListener(OnLeftButtonClick);
            rightButton.onClick.RemoveListener(OnRightButtonClick);
        }

        private async void OnLeftButtonClick()
        {
            await GameLobbyManager.Instance.SetPlayerTeam(PlayerTeam.Local);
        }

        private async void OnRightButtonClick()
        {
            await GameLobbyManager.Instance.SetPlayerTeam(PlayerTeam.Visitor);
        }
    }
}
