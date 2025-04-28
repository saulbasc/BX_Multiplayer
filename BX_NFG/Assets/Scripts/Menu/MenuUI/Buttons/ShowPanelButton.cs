using UnityEngine;

namespace Assets.Scripts.Menu.MenuUI
{
    public class ShowPanelButton : BaseButton
    {
        [SerializeField] private GameObject panel;

        protected override void action() => panel.SetActive(true);
    }
}
