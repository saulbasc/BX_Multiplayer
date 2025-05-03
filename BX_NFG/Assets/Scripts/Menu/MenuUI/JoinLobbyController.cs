
using Assets.Scripts.Connection.Lobbi;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Menu.MenuUI
{
    public class JoinLobbyController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField codeText;
        [SerializeField] private Button joinButton;
        [SerializeField] private Button backButton;

        private void OnEnable()
        {
            joinButton.onClick.AddListener(OnJoinButtonClicked);
            backButton.onClick.AddListener(OnBackButtonPressed);
        }

        private void OnDisable()
        {
            joinButton.onClick.RemoveListener(OnJoinButtonClicked);
            backButton.onClick.RemoveListener(OnBackButtonPressed);
        }

        private async void OnJoinButtonClicked()
        {
            string code = codeText.text;
            code = code.Substring(0, code.Length);
            bool success = await GamePlayersManager.Instance.JoinLobby(code);
            if (success)
            {
                await SceneManager.LoadSceneAsync("Lobby");
            }
        }

        private void OnBackButtonPressed()
        {
            gameObject.SetActive(false);
        }   
    }
}
