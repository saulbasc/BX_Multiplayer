using Assets.Scripts.Game.GameEvents.Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.GameUI
{
    public class GamePassButton : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            PlayerEvents.OnPassAction?.Invoke();
        }
    }
}
