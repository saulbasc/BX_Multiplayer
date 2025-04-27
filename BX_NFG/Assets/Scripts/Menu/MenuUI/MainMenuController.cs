using System;
using Assets.Scripts.Connection.Lobbi;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject joinPanel;
        [SerializeField] private GameObject profilePanel;
        [SerializeField] private GameObject gamePlayPanel;
        [SerializeField] private Button profileButton;
        [SerializeField] private Button joinCodeButton;
        [SerializeField] private Button gamePlayButton;

        private void Start()
        {
            joinPanel.SetActive(false);
        }

        private void OnEnable()
        {
            gamePlayButton.onClick.AddListener(OnGamePlayButtonClicked);
            joinCodeButton.onClick.AddListener(OnJoinCodeButtonClicked);
            profileButton.onClick.AddListener(OnProfileButtonClicked);
        }

        private void OnDisable()
        {
            gamePlayButton.onClick.RemoveListener(OnGamePlayButtonClicked);
            joinCodeButton.onClick.RemoveListener(OnJoinCodeButtonClicked);
            profileButton.onClick.RemoveListener(OnProfileButtonClicked);
        }

        private void OnGamePlayButtonClicked() => gamePlayPanel.SetActive(true);

        private void OnJoinCodeButtonClicked() => joinPanel.SetActive(true);

        private void OnProfileButtonClicked() => profilePanel.SetActive(true);
    }
}
