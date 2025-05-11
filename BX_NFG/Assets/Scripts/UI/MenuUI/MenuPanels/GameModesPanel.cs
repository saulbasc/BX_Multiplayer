
using Assets.Scripts.Commons;
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
        [SerializeField] private Button trainingButton;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;

        public override void Initialize(IUIManager manager)
        {
            menuManager = manager;

            backButton.onClick.AddListener(() => menuManager.ShowPanel(PanelType.MainMenuPanel));
            trainingButton.onClick.AddListener(() => Debug.Log("Training selected"));
            hostButton.onClick.AddListener(() => CreateLobby());
            joinButton.onClick.AddListener(() => menuManager.AddFloatPanel(PanelType.JoinLobbyPanel));
        }

        private async void CreateLobby()
        {
            await LobbyServiceManager.Instance.CreateLobby();
            await SceneManager.LoadSceneAsync(Scenes.Lobby.ToString());
        }
    }
}
