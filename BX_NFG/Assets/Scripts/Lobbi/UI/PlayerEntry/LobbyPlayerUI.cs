using System;
using System.Collections.Generic;

using TMPro;
using UnityEngine;

namespace Assets.Scripts.Lobbi.Players
{
    public class LobbyPlayerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;

        public void SetPlayerName(string playerName)
        {
            if(playerNameText != null)
            {
                playerNameText.text = playerName;
            }
        }
    }
}
