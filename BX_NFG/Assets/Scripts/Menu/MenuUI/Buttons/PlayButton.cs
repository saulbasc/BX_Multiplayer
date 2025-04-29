using UnityEngine;

namespace Assets.Scripts.Menu.MenuUI.Buttons
{
    public class PlayButton : BaseButton
    {
        [SerializeField] private GameObject playPanel;

        protected override void action()
        {
            if (playPanel.activeSelf)
            {
                playPanel.SetActive(false);
            }
            else
            {
                playPanel.SetActive(true);
            }
        }
    }
}
