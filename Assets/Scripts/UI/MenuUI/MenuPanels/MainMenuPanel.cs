using System.Threading.Tasks;
using Assets.Scripts.Core.FireB;
using Assets.Scripts.Core.Models;
using Assets.Scripts.UI.Common;
using Assets.Scripts.UI.MenuUI.MenuPanels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI
{
    public class MainMenuPanel : PanelBase
    {
        public override PanelType PanelType => PanelType.MainMenuPanel;

        [SerializeField] private Button gameModesButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button statsButton;
        [SerializeField] private Button profileButton;
        [SerializeField] private Button rankingButton;

        [SerializeField] private TextMeshProUGUI profileButtonText;

        private void OnEnable()
        {
            SetConfirmButtonText();
            MenuEvents.Instance.OnNameUpdated += SetConfirmButtonText;
        }

        public override void Initialize(IUIManager manager)
        {
            base.manager = manager;

            gameModesButton.onClick.AddListener(() => base.manager.ShowPanel(PanelType.GameModesPanel));
            settingsButton.onClick.AddListener(() => base.manager.AddFloatPanel(PanelType.SettingsPanel));
            statsButton.onClick.AddListener(() => base.manager.ShowPanel(PanelType.StatsPanel));
            profileButton.onClick.AddListener(() => base.manager.AddFloatPanel(PanelType.ProfilePanel));
            rankingButton.onClick.AddListener(() => base.manager.ShowPanel(PanelType.RankingPanel));
        }

        private async Task<User> GetUser()
        {
            return await UserDAO.Instance.select(FirebaseActions.GetCurrentID());
        }
        
        private async void SetConfirmButtonText()
        {
            User user = await GetUser();
            profileButtonText.text = user.Username;
        }
    }
}
