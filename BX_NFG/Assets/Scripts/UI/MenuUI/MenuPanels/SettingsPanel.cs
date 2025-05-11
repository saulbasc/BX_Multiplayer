using Assets.Scripts.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI
{
    public class SettingsPanel : PanelBase
    {
        public override PanelType PanelType => PanelType.SettingsPanel;
        [SerializeField] private Button backButton;

        public override void Initialize(IUIManager manager)
        {
            menuManager = manager;

            backButton.onClick.AddListener(() => menuManager.RemoveFloatPanel(PanelType));
        }
    }
}
