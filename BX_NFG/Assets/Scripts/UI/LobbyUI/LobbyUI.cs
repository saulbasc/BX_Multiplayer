using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Connection;
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
