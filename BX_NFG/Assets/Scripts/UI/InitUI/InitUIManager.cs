using Assets.Scripts.UI.Common;

namespace Assets.Scripts.UI.MenuUI
{
    public class InitUIManager : UIManagerBase
    {
        protected override void StartAction()
        {
            ShowPanel(PanelType.InitLoadingPanel);
        }
    }
}
