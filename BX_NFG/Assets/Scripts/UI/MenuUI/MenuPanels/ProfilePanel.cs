using System;
using Assets.Scripts.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI
{
    public class ProfilePanel : PanelBase
    {
        public override PanelType PanelType => PanelType.ProfilePanel;

        [SerializeField] private Button backButton;
        [SerializeField] private Button changeNameButton;

        public override void Initialize(IUIManager manager)
        {
            menuManager = manager;

            backButton.onClick.AddListener(() => menuManager.ShowPanel(PanelType.MainMenuPanel));
            changeNameButton.onClick.AddListener(() => menuManager.AddFloatPanel(PanelType.ChangeProfileNamePanel));
        }
    }
}
