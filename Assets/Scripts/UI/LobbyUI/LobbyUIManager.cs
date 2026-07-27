using System.Collections;
using Assets.Scripts.Commons;
using Assets.Scripts.Lobbi;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Relay;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.LobbyUI
{
    public class LobbyUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lobbyNameText;

        private bool canStart;

        private LobbyDataManager lobbyDataManager;
        private LobbyActionsManager lobbyActionsManager;
        private ClientRelayManager clientRelayManager;
        private HostRelayManager hostRelayManager;

        [SerializeField] private Button exitButton;
        [SerializeField] private Button readyButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button startButton;

        private System.Action onLobbyCancelAction;
        private System.Action onLobbyReadyAction;

        private void OnEnable()
        {
            LobbyEvents.Instance.OnLobbyReadyToStart += () => canStart = true;
            StartCoroutine(InitializeManagersAndUI());
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

        private IEnumerator InitializeManagersAndUI()
        {
            while (lobbyDataManager == null || lobbyActionsManager == null ||
                   clientRelayManager == null || hostRelayManager == null)
            {
                lobbyDataManager = FindFirstObjectByType<LobbyDataManager>();
                lobbyActionsManager = FindFirstObjectByType<LobbyActionsManager>();
                clientRelayManager = FindFirstObjectByType<ClientRelayManager>();
                hostRelayManager = FindFirstObjectByType<HostRelayManager>();
                yield return null;
            }

            exitButton.onClick.AddListener(OnExitButtonClicked);
            readyButton.onClick.AddListener(OnReadyButtonClicked);
            cancelButton.onClick.AddListener(OnCancelButtonClicked);
            startButton.onClick.AddListener(OnStartButtonClick);

            while (!canStart)
            {
                yield return null;
            }

            lobbyNameText.text = lobbyDataManager.GetLobbyCode();

            onLobbyCancelAction = () => startButton.gameObject.SetActive(false);
            LobbyEvents.Instance.OnLobbyCancel += onLobbyCancelAction;

            if (lobbyDataManager.IsLocalPlayerHost())
            {
                onLobbyReadyAction = () => startButton.gameObject.SetActive(true);
                LobbyEvents.Instance.OnLobbyReady += onLobbyReadyAction;
            }
        }

        private async void OnExitButtonClicked()
        {
            await lobbyActionsManager.ExitLobby();
            SceneManager.LoadScene(Scenes.MenuScene.ToString());
        }

        private async void OnReadyButtonClicked()
        {
            bool success = await lobbyActionsManager.SetLocalLobbyPlayerReadyStatus(true);
            if (success)
            {
                readyButton.gameObject.SetActive(false);
                cancelButton.gameObject.SetActive(true);
            }
        }

        private async void OnCancelButtonClicked()
        {
            bool success = await lobbyActionsManager.SetLocalLobbyPlayerReadyStatus(false);
            if (success)
            {
                readyButton.gameObject.SetActive(true);
                cancelButton.gameObject.SetActive(false);
            }
        }

        private async void OnStartButtonClick()
        {
            LobbyEvents.Instance.RaiserLobbyStart();
            if (lobbyDataManager.IsLocalPlayerHost())
            {
                await hostRelayManager.StartRelayServer();
            }
            else
            {
                await clientRelayManager.JoinRelayServer();
            }
        }
    }
}
