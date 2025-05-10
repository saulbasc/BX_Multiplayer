using Assets.Scripts.Connection.Lobbi;
using Assets.Scripts.Core.FireB;
using Assets.Scripts.Core.Models;
using Assets.Scripts.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI
{
    public class JoinLobbyPanel : PanelBase
    {
        public override PanelType PanelType => PanelType.JoinLobbyPanel;

        [SerializeField] private Button backButton;
        [SerializeField] private Button confirmLobbyCodeButton;
        [SerializeField] private TMP_InputField lobbyCodeInput;

        public override void Initialize(MenuUIManager manager)
        {
            menuManager = manager;

            backButton.onClick.AddListener(() => menuManager.RemoveFloatPanel(PanelType));
            confirmLobbyCodeButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        private async void OnConfirmButtonClicked()
        {
            menuManager.RemoveFloatPanel(PanelType);
            string lobbyCode = lobbyCodeInput.text;
            bool success = await LobbyManager.Instance.JoinLobby(lobbyCode);
            if (!success) menuManager.AddFloatPanel(PanelType.LobbyNotFoundPanel);
        }
    }
}
