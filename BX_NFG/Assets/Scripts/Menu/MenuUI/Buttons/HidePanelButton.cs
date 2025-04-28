using UnityEngine;

namespace Assets.Scripts.Menu.MenuUI.Buttons
{
    public class HidePanelButton : BaseButton
    {
        [SerializeField] private GameObject panel;
        protected override void action() => panel.SetActive(false);
    }
}
