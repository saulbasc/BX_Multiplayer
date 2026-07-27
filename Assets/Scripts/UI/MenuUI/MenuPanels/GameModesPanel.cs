
using Assets.Scripts.Commons;
using Assets.Scripts.Lobbi.LobbyManager;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.UI.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI
{
    public class GameModesPanel : PanelBase
    {
        public override PanelType PanelType => PanelType.GameModesPanel;

        [SerializeField] private Button backButton;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;

        public override void Initialize(IUIManager manager)
        {
            base.manager = manager;

            backButton.onClick.AddListener(() => base.manager.ShowPanel(PanelType.MainMenuPanel));
            hostButton.onClick.AddListener(() => CreateLobby());
            joinButton.onClick.AddListener(() => base.manager.AddFloatPanel(PanelType.JoinLobbyPanel));
        }

        private async void CreateLobby()
        {
            LobbyIntent.Instance.IsCreatingLobby = true;
            await SceneManager.LoadSceneAsync(Scenes.Lobby.ToString());
        }
    }
}
