using UnityEngine;

namespace Assets.Scripts.Animations
{
    public class LobbyPanelAnimation : MonoBehaviour
    {
        private void Start()
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            LeanTween.scale(gameObject, Vector3.one, 0.2f)
            .setEase(LeanTweenType.easeOutBounce);
        }

        private void OnDestroy()
        {
            LeanTween.scale(gameObject, Vector3.zero, 0.1f)
            .setEase(LeanTweenType.easeOutBounce);
        }
    }
}
