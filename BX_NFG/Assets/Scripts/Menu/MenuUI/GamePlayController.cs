
using System;
using Assets.Scripts.Connection.Lobbi;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu.MenuUI
{
    public class GamePlayController : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button joinGameButton;
        [SerializeField] private Button backButton;

        [SerializeField] private GameObject joinPanel;

        private void OnEnable()
        {
            startGameButton.onClick.AddListener(OnStartGameButtonClicked);
            joinGameButton.onClick.AddListener(OnJoinGameButtonClicked);
            backButton.onClick.AddListener(OnBackButtonPressed);
        }

        private void OnDisable()
        {
            startGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
            joinGameButton.onClick.RemoveListener(OnJoinGameButtonClicked);
            backButton.onClick.RemoveListener(OnBackButtonPressed);
        }

        private void OnBackButtonPressed()
        {
            gameObject.SetActive(false);
        }

        private void OnJoinGameButtonClicked()
        {
            joinPanel.SetActive(true);
        }

        private async void OnStartGameButtonClicked()
        {
            await GameLobbyManager.Instance.CreateLobby();
        }
    }
}
