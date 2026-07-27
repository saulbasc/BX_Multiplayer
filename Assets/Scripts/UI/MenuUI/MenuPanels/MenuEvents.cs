using System;
using Assets.Scripts.Commons;

namespace Assets.Scripts.UI.MenuUI.MenuPanels
{
    public class MenuEvents : DefaultSingleton<MenuEvents>
    {
        public event Action OnNameUpdated;
        public void RaiseNameUpdated() => OnNameUpdated?.Invoke();
    }
}
