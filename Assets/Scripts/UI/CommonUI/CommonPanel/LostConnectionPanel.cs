using Assets.Scripts.UI.MenuUI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Common.CommonPanel
{
    public class LostConnectionPanel : PanelBase
    {
        public override PanelType PanelType => PanelType.ConnectionLostPanel;
        [SerializeField] private Button backButton;
        [SerializeField] private Button confirmButton;

        public override void Initialize (IUIManager manager)
        { 
            base.manager = manager;

            backButton.onClick.AddListener(() => base.manager.RemoveFloatPanel(PanelType));
            confirmButton.onClick.AddListener(() => base.manager.RemoveFloatPanel(PanelType));
        }
    }
}
