using UnityEngine;

namespace Assets.Scripts.Menu.MenuUI
{
    public class SceneAnimator : MonoBehaviour
    {
        private void Start()
        {
            transform.localScale = new Vector3(1.03f, 1.03f, 1.03f);
            CommonAnimations.SimpleRebound(gameObject);
        }
    }
}
