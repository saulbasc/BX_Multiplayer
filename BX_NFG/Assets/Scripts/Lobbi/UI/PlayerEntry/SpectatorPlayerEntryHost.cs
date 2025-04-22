
using Assets.Scripts.Connection.Lobbi;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.UI.PlayerEntry;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Lobbi.UI
{
    public class SpectatorPlayerEntryHost : PlayerEntryHost
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

        private void OnLeftButtonClick()
        {
            ChangeTeam(PlayerTeam.Local);
        }

        private void OnRightButtonClick()
        {
            ChangeTeam(PlayerTeam.Visitor);
        }
    }
}
