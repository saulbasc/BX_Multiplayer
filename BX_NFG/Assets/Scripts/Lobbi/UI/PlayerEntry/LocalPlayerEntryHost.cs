
using Assets.Scripts.Lobbi.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Lobbi.UI.PlayerEntry
{
    public class LocalPlayerEntryHost : PlayerEntryHost
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

        private void OnRightButtonClick()
        {
            ChangeTeam(PlayerTeam.Spectator);
        }
    }
}
