using Assets.Scripts.Handlers;
using Assets.Scripts.Lobbi;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Relay;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.LobbyUI
{
    public class LobbyUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lobbyNameText;
        
        [SerializeField] private Button exitButton;
        [SerializeField] private Button readyButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button startButton;

        private void OnEnable()
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
            readyButton.onClick.AddListener(OnReadyButtonClicked);
            cancelButton.onClick.AddListener(OnCancelButtonClicked);
            lobbyNameText.text = LobbyDataManager.Instance.GetLobbyCode();

            GameLobbyEvents.OnLobbyCancel += OnLobbyCancel;

            InitializeLobbyController();
        }

        private void InitializeLobbyController()
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
            await LobbyServiceManager.Instance.DisconnectFromLobby();
        }

        private async void OnReadyButtonClicked()
        {
            await SafeAsyncFunctionsHandler.ExecuteAsync(async () => 
            {
                bool success = await LobbyPlayersManager.Instance.SetPlayerReadyAsync(true);
                if (success)
                {
                    readyButton.gameObject.SetActive(false);
                    cancelButton.gameObject.SetActive(true);
                }
            });
        }

        private async void OnCancelButtonClicked()
        {
            await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                bool success = await LobbyPlayersManager.Instance.SetPlayerReadyAsync(false);
                if (success)
                {
                    readyButton.gameObject.SetActive(true);
                    cancelButton.gameObject.SetActive(false);
                }
            });
        }

        private async void OnStartButtonClick()
        {
            await SafeAsyncFunctionsHandler.ExecuteAsync( async () =>
            {
                if (LobbyDataManager.Instance.IsHost())
                {
                    await HostRelayManager.Instance.StartRelayServer();
                }
                else
                {
                    await ClientRelayManager.Instance.JoinRelayServer();
                }
            });
        }
    }
}
