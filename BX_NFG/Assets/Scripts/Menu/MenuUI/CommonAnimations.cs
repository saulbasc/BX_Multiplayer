using UnityEngine;

namespace Assets.Scripts.Menu.MenuUI
{
    public static class CommonAnimations
    {
        public static void OnPressDownButton(GameObject gameObject)
        {
            LeanTween.scale(gameObject, new Vector3(0.9f, 0.9f, 1.0f), 0.1f).setEaseInOutBack();
        }

        public static void OnPressUpButton(GameObject gameObject)
        {
            LeanTween.scale(gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.1f).setEaseInOutBack();
        }
    }
}
