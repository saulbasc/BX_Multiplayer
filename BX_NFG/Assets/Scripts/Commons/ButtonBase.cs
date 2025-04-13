using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Buttons
{
    public abstract class ButtonBase: MonoBehaviour
    {

        [SerializeField] private Button button;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        public abstract void OnClick();
    }
}
