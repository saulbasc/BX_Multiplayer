using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Menu.MenuUI
{
    public abstract class BaseButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            CommonAnimations.OnPressDownButton(gameObject);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            CommonAnimations.OnPressUpButton(gameObject);
            action();
        }

        protected abstract void action();
    }
}
