
namespace Assets.Scripts.UI.Common
{
    public interface IUIManager
    {
        void ShowPanel(PanelType type);
        void AddFloatPanel(PanelType type);
        void RemoveFloatPanel(PanelType type);
    }
}
