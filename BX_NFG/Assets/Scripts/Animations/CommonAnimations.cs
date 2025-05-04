
using UnityEngine;

namespace Assets.Scripts.Menu.MenuUI
{
    public static class CommonAnimations
    {
        public static void OnPressDownButton(GameObject gameObject)
        {
            LeanTween.scale(gameObject, new Vector3(0.95f, 0.95f, 0.95f), 0.1f).setEaseInOutBack();
        }

        public static void OnPressUpButton(GameObject gameObject)
        {
            LeanTween.scale(gameObject, Vector3.one, 0.1f).setEaseInOutBack();
        }

        public static void SimpleRebound(GameObject gameObject)
        {
            LeanTween.scale(gameObject, Vector3.one, 0.5f)
            .setEase(LeanTweenType.easeOutBounce);
        }

        public static void ShowPanel(GameObject gameObject)
        { 
            gameObject.transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, Vector3.one, 0.3f).setEaseOutBack();
        }

        public static void HidePanel(GameObject gameObject)
        {
            LeanTween.scale(gameObject, Vector3.zero, 0.3f).setEaseOutBack();
        }
    }
}
