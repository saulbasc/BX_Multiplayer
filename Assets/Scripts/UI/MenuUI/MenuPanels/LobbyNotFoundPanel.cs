
using Assets.Scripts.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI
{
    public class LobbyNotFoundPanel : PanelBase
    {
        public override PanelType PanelType => PanelType.LobbyNotFoundPanel;
        [SerializeField] private Button backButton;
        [SerializeField] private Button confirmButton;

        public override void Initialize(IUIManager manager)
        {
            base.manager = manager;

            backButton.onClick.AddListener(() => base.manager.RemoveFloatPanel(PanelType));
            confirmButton.onClick.AddListener(() => base.manager.RemoveFloatPanel(PanelType));
        }
    }
}
