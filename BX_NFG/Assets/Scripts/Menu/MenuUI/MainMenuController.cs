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
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private Button backButton;

        [SerializeField] private Button joinCodeButton;
        [SerializeField] private TextMeshProUGUI codeText;

        private void Start()
        {
            joinPanel.SetActive(false);
        }

        private void OnEnable()
        {
            hostButton.onClick.AddListener(OnHostButtonClicked);
            joinButton.onClick.AddListener(OnJoinButtonClicked);
            backButton.onClick.AddListener(OnBackButtonClicked);
            joinCodeButton.onClick.AddListener(OnJoinCodeButtonClicked);
        }

        private void OnDisable()
        {
            hostButton.onClick.RemoveListener(OnHostButtonClicked);
            joinButton.onClick.RemoveListener(OnJoinButtonClicked);
            backButton.onClick.RemoveListener(OnBackButtonClicked);
            joinCodeButton.onClick.RemoveListener(OnJoinCodeButtonClicked);
        }


        private async void OnHostButtonClicked()
        {
            bool success = await GameLobbyManager.Instance.CreateLobby();
            if (success)
            {
                await SceneManager.LoadSceneAsync("Lobby");
            }
        }

        private void OnJoinButtonClicked()
        {
            menuPanel.SetActive(false);
            joinPanel.SetActive(true);
        }

        private void OnBackButtonClicked()
        {
            menuPanel.SetActive(true);
            joinPanel.SetActive(false);
        }

        private async void OnJoinCodeButtonClicked()
        {
            string code = codeText.text;
            code = code.Substring(0, code.Length - 1);
            bool success = await GameLobbyManager.Instance.JoinLobby(code);
            if(success)
            {
                await SceneManager.LoadSceneAsync("Lobby");
            }
        }
    }
}
