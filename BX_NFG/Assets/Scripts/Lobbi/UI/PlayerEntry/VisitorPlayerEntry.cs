
using Assets.Scripts.Connection.Lobbi;
using Assets.Scripts.Lobbi.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Lobbi.UI.PlayerEntry
{
    public class VisitorPlayerEntry : MonoBehaviour
    {
        [SerializeField] private Button leftButton;

        private void OnEnable()
        {
            leftButton.onClick.AddListener(OnLeftButtonClick);
        }

        private void OnDisable()
        {
            leftButton.onClick.RemoveListener(OnLeftButtonClick);
        }

        private async void OnLeftButtonClick()
        {
            await GameLobbyManager.Instance.SetPlayerTeam(PlayerTeam.Spectator);
        }
    }
}
