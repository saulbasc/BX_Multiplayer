using System.Collections.Generic;
using Assets.Scripts.Connection.Lobbi;
using Assets.Scripts.Lobbi;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.LobbyUI
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lobbyNameText;

        private void Start()
        {
            lobbyNameText.text = GameLobbyManager.Instance.GetLobbyCode();   
        }
    }
}
