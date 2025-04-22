
using Assets.Scripts.Lobbi.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Lobbi.UI.PlayerEntry
{
    public class VisitorPlayerEntryHost : PlayerEntryHost
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

        private void OnLeftButtonClick()
        {
            ChangeTeam(PlayerTeam.Spectator);
        }
    }
}
