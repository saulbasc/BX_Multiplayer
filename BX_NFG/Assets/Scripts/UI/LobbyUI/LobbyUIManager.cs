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

        private void OnEnable()
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
            readyButton.onClick.AddListener(OnReadyButtonClicked);
            cancelButton.onClick.AddListener(OnCancelButtonClicked);
            lobbyNameText.text = LobbyDataManager.Instance.GetLobbyCode();

            LobbyEvents.Instance.OnLobbyCancel += () => startButton.gameObject.SetActive(false);

            if (LobbyDataManager.Instance.IsLocalPlayerHost())
            {
                LobbyEvents.Instance.OnLobbyReady += () => startButton.gameObject.SetActive(true);
                startButton.onClick.AddListener(OnStartButtonClick);
            }
        }

        private void OnDisable()
        {
            exitButton.onClick.RemoveListener(OnExitButtonClicked);
            readyButton.onClick.RemoveListener(OnReadyButtonClicked);
            cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
            startButton.onClick.RemoveListener(OnStartButtonClick);

            LobbyEvents.Instance.OnLobbyReady -= () => startButton.gameObject.SetActive(true);
            LobbyEvents.Instance.OnLobbyCancel -= () => startButton.gameObject.SetActive(false);
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
            if (LobbyDataManager.Instance.IsLocalPlayerHost())
            {
                await LobbyActionsManager.Instance.StartLobbyMatchAsHost();
            }
            else
            {
                await LobbyActionsManager.Instance.StartLobbyMatchAsClient();
            }
        }
    }
}
