
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
        [SerializeField] private Button trainingButton;
        [SerializeField] private Button gameButton;

        [SerializeField] private GameObject joinPanel;

        private void OnEnable()
        {
            startGameButton.onClick.AddListener(OnStartGameButtonClicked);
            joinGameButton.onClick.AddListener(OnJoinGameButtonClicked);
            trainingButton.onClick.AddListener(OnTrainingButtonPressed);
            gameButton.onClick.AddListener(OnGameButtonPressed);
        }

        private void OnDisable()
        {
            startGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
            joinGameButton.onClick.RemoveListener(OnJoinGameButtonClicked);
            trainingButton.onClick.RemoveListener(OnTrainingButtonPressed);
            gameButton.onClick.RemoveListener(OnGameButtonPressed);
        }

        private void OnTrainingButtonPressed() { }

        private void OnGameButtonPressed() {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        private void OnJoinGameButtonClicked() => joinPanel.SetActive(true);

        private async void OnStartGameButtonClicked() => await GameLobbyManager.Instance.CreateLobby();
    }
}
