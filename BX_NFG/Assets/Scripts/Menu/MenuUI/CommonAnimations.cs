using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Menu.MenuUI
{
    public static class CommonAnimations
    {
        public static void OnPressDownButton(GameObject gameObject)
        {
            LeanTween.scale(gameObject, Vector3.one, 0.1f).setEaseInOutBack();
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
    }
}
