using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Menu.MenuUI;
using Assets.Scripts.Sound;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Assets.Scripts.UI.CommonUI
{
    public class ButtonBaseSilent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            CommonAnimations.OnPressDownButton(gameObject);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            CommonAnimations.OnPressUpButton(gameObject);
        }
    }
}
