
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

        public override void Initialize(MenuUIManager manager)
        {
            menuManager = manager;

            backButton.onClick.AddListener(() => menuManager.RemoveFloatPanel(PanelType));
            confirmButton.onClick.AddListener(() => menuManager.RemoveFloatPanel(PanelType));
        }
    }
}
