using System;
using Assets.Scripts.Core.FireB;
using Assets.Scripts.Core.Models;
using System.Threading.Tasks;
using Assets.Scripts.UI.Common;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.UI.MenuUI.MenuPanels;

namespace Assets.Scripts.UI.MenuUI
{
    public class ProfilePanel : PanelBase
    {
        public override PanelType PanelType => PanelType.ProfilePanel;

        [SerializeField] private Button backButton;
        [SerializeField] private Button changeNameButton;
        [SerializeField] private TextMeshProUGUI nameText;

        public override void Initialize(IUIManager manager)
        {
            SetUserText();
            base.manager = manager;

            backButton.onClick.AddListener(() => base.manager.RemoveFloatPanel(PanelType));
            changeNameButton.onClick.AddListener(() => base.manager.AddFloatPanel(PanelType.ChangeProfileNamePanel));
        }

        private void OnEnable()
        {
            MenuEvents.Instance.OnNameUpdated += SetUserText;
        }

        private void OnDisable()
        {
            MenuEvents.Instance.OnNameUpdated -= SetUserText;
        }

        private async Task<User> GetUser()
        {
            return await UserDAO.Instance.select(FirebaseActions.GetCurrentID());
        }

        private async void SetUserText()
        {
            User user = await GetUser();
            nameText.text = user.Username;
        }
    }
}
