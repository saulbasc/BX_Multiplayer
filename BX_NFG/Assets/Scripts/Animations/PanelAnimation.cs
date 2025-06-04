using Assets.Scripts.Menu.MenuUI;
using UnityEngine;

namespace Assets.Scripts.Animations
{
    public class PanelAnimation : MonoBehaviour
    {
        private void OnEnable()
        {
            transform.localScale = new Vector3(1.03f, 1.03f, 1.03f);
            CommonAnimations.SimpleRebound(gameObject);
        }

        private void OnDisable()
        {
            LeanTween.scale(gameObject, Vector3.zero, 0.1f)
            .setEase(LeanTweenType.easeOutBounce);
        }
    }
}
