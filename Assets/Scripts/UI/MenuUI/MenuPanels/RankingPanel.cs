using Assets.Scripts.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI
{
    public class RankingPanel : PanelBase
    {
        public override PanelType PanelType => PanelType.RankingPanel;
        [SerializeField] private Button backButton;

        public override void Initialize(IUIManager manager)
        {
            base.manager = manager;

            backButton.onClick.AddListener(() => base.manager.ShowPanel(PanelType.MainMenuPanel));
        }
    }
}
