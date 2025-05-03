
using System;
using Assets.Scripts.Connection.Lobbi;
using Assets.Scripts.Lobbi;
using Assets.Scripts.Lobbi.Logic;
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

            GameLobbyEvents.OnLobbyCancel += OnLobbyCancel;

            inicializeLobbyController();
        }

        private void inicializeLobbyController()
        {
            if (AuthenticationService.Instance.PlayerId == LobbyDataManager.Instance.GetHostID())
            {
                GameLobbyEvents.OnLobbyReady += OnLobbyReady;
                startButton.onClick.AddListener(OnStartButtonClick);
            }
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
            await LobbyServiceHandler.Instance.DisconnectFromLobby();
        }

        private async void OnReadyButtonClicked()
        {
            bool success = await LobbyPlayersManager.Instance.SetPlayerReadyAsync(true);
            if(success)
            {
                readyButton.gameObject.SetActive(false);
                cancelButton.gameObject.SetActive(true);
            }
        }

        private async void OnCancelButtonClicked()
        {
            bool success = await LobbyPlayersManager.Instance.SetPlayerReadyAsync(false);
            if (success)
            {
                readyButton.gameObject.SetActive(true);
                cancelButton.gameObject.SetActive(false);
            }
        }

        private async void OnStartButtonClick()
        {
            await RelayManager.Instance.StartRelayServer();
        }
    }
}
