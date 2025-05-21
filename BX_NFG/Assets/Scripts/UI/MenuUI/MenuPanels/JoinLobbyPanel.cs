using Assets.Scripts.Commons;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI
{
    public class JoinLobbyPanel : PanelBase
    {
        public override PanelType PanelType => PanelType.JoinLobbyPanel;

        [SerializeField] private Button backButton;
        [SerializeField] private Button confirmLobbyCodeButton;
        [SerializeField] private TMP_InputField lobbyCodeInput;

        public override void Initialize(IUIManager manager)
        {
            menuManager = manager;

            backButton.onClick.AddListener(() => menuManager.RemoveFloatPanel(PanelType));
            confirmLobbyCodeButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        private async void OnConfirmButtonClicked()
        {
            menuManager.RemoveFloatPanel(PanelType);
            string lobbyCode = lobbyCodeInput.text;
            bool success = await LobbyServiceManager.Instance.JoinLobby(lobbyCode);
            if (success)
            {
                await SceneManager.LoadSceneAsync(Scenes.Lobby.ToString());
            }
            else
            {
                menuManager.AddFloatPanel(PanelType.LobbyNotFoundPanel);
            }
        }
    }
}
