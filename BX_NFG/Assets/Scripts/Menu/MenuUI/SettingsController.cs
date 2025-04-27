using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu.MenuUI
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField] private Button saveButton;

        private void OnEnable()
        {
            saveButton.onClick.AddListener(SaveConfig);
        }

        private void OnDisable()
        {
            saveButton.onClick.RemoveListener(SaveConfig);
        }

        private void SaveConfig()
        {
            LeanTween.scale(gameObject, Vector3.zero, 0.3f).setEaseInOutBack()
                .setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }
    }
}
