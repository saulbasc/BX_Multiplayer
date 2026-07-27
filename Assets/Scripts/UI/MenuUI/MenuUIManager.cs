using Assets.Scripts.UI.Common;

namespace Assets.Scripts.UI.MenuUI
{
    public class MenuUIManager : UIManagerBase
    {
        protected override void StartAction()
        {
            ShowPanel(PanelType.MainMenuPanel);
        }
    }
}
