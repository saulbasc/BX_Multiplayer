using System;
using System.Threading.Tasks;
using Assets.Scripts.Connection.Lobbi;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.LobbyUI
{
    public class LobbyController : MonoBehaviour
    {
        [SerializeField] private Button readyButton;

        private void OnEnable()
        {
            readyButton.onClick.AddListener(OnReadyButtonClicked);
        }

        private void OnDisable()
        {
            readyButton.onClick.RemoveListener(OnReadyButtonClicked);
        }

        private async void OnReadyButtonClicked()
        {
            bool success = await GameLobbyManager.Instance.SetPlayerReady();
            if(success)
            {
                readyButton.gameObject.SetActive(false);
            }
        }
    }
}
