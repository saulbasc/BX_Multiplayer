using Assets.Scripts.Sound;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Menu.MenuUI
{
    public class ButtonBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            CommonAnimations.OnPressDownButton(gameObject);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SoundEvents.Instance.RaiseClickSound();
            CommonAnimations.OnPressUpButton(gameObject);
        }
    }
}
