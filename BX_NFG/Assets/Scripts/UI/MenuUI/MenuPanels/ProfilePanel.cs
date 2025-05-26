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
            base.manager = manager;

            backButton.onClick.AddListener(() => base.manager.ShowPanel(PanelType.MainMenuPanel));
            changeNameButton.onClick.AddListener(() => base.manager.AddFloatPanel(PanelType.ChangeProfileNamePanel));
        }
    }
}
