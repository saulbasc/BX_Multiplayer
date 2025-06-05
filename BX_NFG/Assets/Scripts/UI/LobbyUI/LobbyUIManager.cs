using Assets.Scripts.Lobbi;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Relay;
using TMPro;
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

        private System.Action onLobbyCancelAction;
        private System.Action onLobbyReadyAction;

        private void OnEnable()
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
            readyButton.onClick.AddListener(OnReadyButtonClicked);
            cancelButton.onClick.AddListener(OnCancelButtonClicked);
            startButton.onClick.AddListener(OnStartButtonClick);
            lobbyNameText.text = LobbyDataManager.Instance.GetLobbyCode();

            onLobbyCancelAction = () => startButton.gameObject.SetActive(false);
            LobbyEvents.Instance.OnLobbyCancel += onLobbyCancelAction;

            if (LobbyDataManager.Instance.IsLocalPlayerHost())
            {
                onLobbyReadyAction = () => startButton.gameObject.SetActive(true);
                LobbyEvents.Instance.OnLobbyReady += onLobbyReadyAction;
            }
        }

        private void OnDisable()
        {
            exitButton.onClick.RemoveListener(OnExitButtonClicked);
            readyButton.onClick.RemoveListener(OnReadyButtonClicked);
            cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
            startButton.onClick.RemoveListener(OnStartButtonClick);

            if (onLobbyCancelAction != null)
                LobbyEvents.Instance.OnLobbyCancel -= onLobbyCancelAction;

            if (onLobbyReadyAction != null)
                LobbyEvents.Instance.OnLobbyReady -= onLobbyReadyAction;
        }

        private async void OnExitButtonClicked()
        {
            await LobbyActionsManager.Instance.ExitLobby();
        }

        private async void OnReadyButtonClicked()
        {
            bool success = await LobbyActionsManager.Instance.SetLocalLobbyPlayerReadyStatus(true);
            if (success)
            {
                readyButton.gameObject.SetActive(false);
                cancelButton.gameObject.SetActive(true);
            }
        }

        private async void OnCancelButtonClicked()
        {
            bool success = await LobbyActionsManager.Instance.SetLocalLobbyPlayerReadyStatus(false);
            if (success)
            {
                readyButton.gameObject.SetActive(true);
                cancelButton.gameObject.SetActive(false);
            }
        }

        private async void OnStartButtonClick()
        {
            Debug.Log("Start button clicked");
            LobbyEvents.Instance.RaiserLobbyStart();
            if (LobbyDataManager.Instance.IsLocalPlayerHost())
            {
                await HostRelayManager.Instance.StartRelayServer();
            }
            else
            {
                await ClientRelayManager.Instance.JoinRelayServer();
            }
        }
    }
}
