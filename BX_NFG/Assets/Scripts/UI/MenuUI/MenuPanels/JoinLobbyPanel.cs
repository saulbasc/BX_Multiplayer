using Assets.Scripts.Commons;
using Assets.Scripts.Lobbi.LobbyManager;
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
            base.manager = manager;

            backButton.onClick.AddListener(() => base.manager.RemoveFloatPanel(PanelType));
            confirmLobbyCodeButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        private async void OnConfirmButtonClicked()
        {
            manager.RemoveFloatPanel(PanelType);
            LobbyIntent.Instance.IsCreatingLobby = false;
            LobbyIntent.Instance.JoinCode = lobbyCodeInput.text;
            await SceneManager.LoadSceneAsync(Scenes.Lobby.ToString());
        }
    }
}
