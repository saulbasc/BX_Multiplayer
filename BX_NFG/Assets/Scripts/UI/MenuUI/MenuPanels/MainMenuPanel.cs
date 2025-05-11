
using System.Threading.Tasks;
using Assets.Scripts.Core.FireB;
using Assets.Scripts.Core.Models;
using Assets.Scripts.UI.Common;
using TMPro;
using Unity.Android.Types;
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
        }

        public override void Initialize(IUIManager manager)
        {
            menuManager = manager;

            gameModesButton.onClick.AddListener(() => menuManager.ShowPanel(PanelType.GameModesPanel));
            settingsButton.onClick.AddListener(() => menuManager.AddFloatPanel(PanelType.SettingsPanel));
            statsButton.onClick.AddListener(() => menuManager.ShowPanel(PanelType.StatsPanel));
            profileButton.onClick.AddListener(() => menuManager.ShowPanel(PanelType.ProfilePanel));
            rankingButton.onClick.AddListener(() => menuManager.ShowPanel(PanelType.RankingPanel));
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
