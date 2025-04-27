using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject joinPanel;
        [SerializeField] private GameObject profilePanel;
        [SerializeField] private GameObject gamePlayPanel;
        [SerializeField] private GameObject settingsPanel;

        [SerializeField] private Button profileButton;
        [SerializeField] private Button joinCodeButton;
        [SerializeField] private Button gamePlayButton;
        [SerializeField] private Button settingsButton;

        private void Start()
        {
            joinPanel.SetActive(false);
        }

        private void OnEnable()
        {
            gamePlayButton.onClick.AddListener(OnGamePlayButtonClicked);
            joinCodeButton.onClick.AddListener(OnJoinCodeButtonClicked);
            profileButton.onClick.AddListener(OnProfileButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        }

        private void OnDisable()
        {
            gamePlayButton.onClick.RemoveListener(OnGamePlayButtonClicked);
            joinCodeButton.onClick.RemoveListener(OnJoinCodeButtonClicked);
            profileButton.onClick.RemoveListener(OnProfileButtonClicked);
            settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
        }

        private void OnSettingsButtonClicked()
        {
            LeanTween.scale(settingsButton.gameObject, new Vector3(0.95f, 0.95f, 1), 0.1f).setEaseInOutBack()
                .setOnComplete(() =>
                {
                    LeanTween.scale(settingsButton.gameObject, new Vector3(1.0f, 1.0f, 1), 0.2f).setEaseInOutBack()
                    .setOnComplete(() =>
                    {
                        settingsPanel.SetActive(true);
                        settingsPanel.transform.localScale = Vector3.zero;
                        LeanTween.scale(settingsPanel, Vector3.one, 0.3f).setEaseOutBack();
                    });
                });
        }

        private void OnGamePlayButtonClicked() => gamePlayPanel.SetActive(true);

        private void OnJoinCodeButtonClicked() => joinPanel.SetActive(true);

        private void OnProfileButtonClicked() => profilePanel.SetActive(true);

        private void animate()
        {
        }
    }
}
