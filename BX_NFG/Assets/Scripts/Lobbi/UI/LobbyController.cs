
using System;
using Assets.Scripts.Connection.Lobbi;
using Assets.Scripts.Lobbi;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Lobbi.GameLobbyEvents;

namespace Assets.Scripts.UI.LobbyUI
{
    public class LobbyController : MonoBehaviour
    {
        [SerializeField] private Button exitButton;
        [SerializeField] private Button readyButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button startButton;

        private void OnEnable()
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
            readyButton.onClick.AddListener(OnReadyButtonClicked);
            cancelButton.onClick.AddListener(OnCancelButtonClicked);

            if (AuthenticationService.Instance.PlayerId == LobbyManager.Instance.GetHostID())
            {
                GameLobbyEvents.OnLobbyReady += OnLobbyReady;
                startButton.onClick.AddListener(OnStartButtonClick);
            }
            GameLobbyEvents.OnLobbyCancel += OnLobbyCancel;
        }

        private void OnDisable()
        {
            exitButton.onClick.RemoveListener(OnExitButtonClicked);
            readyButton.onClick.RemoveListener(OnReadyButtonClicked);
            cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
            startButton.onClick.RemoveListener(OnStartButtonClick);

            GameLobbyEvents.OnLobbyReady -= OnLobbyReady;
            GameLobbyEvents.OnLobbyCancel -= OnLobbyCancel;
        }

        private void OnLobbyReady()
        {
            startButton.gameObject.SetActive(true);
        }

        private void OnLobbyCancel()
        {
            startButton.gameObject.SetActive(false);
        }

        private async void OnExitButtonClicked()
        {
            await GameLobbyManager.Instance.DisconnectFromLobby();
        }

        private async void OnReadyButtonClicked()
        {
            bool success = await GameLobbyManager.Instance.SetPlayerReady();
            if(success)
            {
                readyButton.gameObject.SetActive(false);
                cancelButton.gameObject.SetActive(true);
            }
        }

        private async void OnCancelButtonClicked()
        {
            bool success = await GameLobbyManager.Instance.SetPlayerNotReady();
            if (success)
            {
                readyButton.gameObject.SetActive(true);
                cancelButton.gameObject.SetActive(false);
            }
        }

        private async void OnStartButtonClick()
        {
            await GameLobbyManager.Instance.StartRelayServer();
        }
    }
}
