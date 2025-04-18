using Assets.Scripts.Connection.Lobbi;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.LobbyUI
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lobbyNameText;

        private void Start()
        {
            lobbyNameText.text = $"Lobby code: {GameLobbyManager.Instance.GetLobbyCode()}";
        }
    }
}
