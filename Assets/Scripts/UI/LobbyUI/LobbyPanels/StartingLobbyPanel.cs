using System;
using Assets.Scripts.Lobbi;
using UnityEngine;

namespace Assets.Scripts.UI.LobbyUI.LobbyPanels
{
    public class StartingLobbyPanel : MonoBehaviour
    {
        [SerializeField] private GameObject startingPanel;

        private void Awake()
        {
            startingPanel.SetActive(false);
            LobbyEvents.Instance.OnLobbyStart += OnLobbyStart;
        }

        private void OnDestroy()
        {
            if (LobbyEvents.Instance != null)
            {
                LobbyEvents.Instance.OnLobbyStart -= OnLobbyStart;
            }
        }

        private void OnLobbyStart()
        {
            startingPanel.SetActive(true);
        }
    }
}
