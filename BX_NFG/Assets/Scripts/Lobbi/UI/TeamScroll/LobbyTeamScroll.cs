
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Connection.Lobbi;
using Assets.Scripts.Lobbi.Data;
using TMPro;

namespace Assets.Scripts.Lobbi.UI.TeamScroll
{
    //An Abstract class that handles the instantiation and management of player scrolls in the lobby.
    public abstract class LobbyTeamScroll : MonoBehaviour
    {
        [SerializeField] protected GameObject playerPanelPrefab;
        [SerializeField] protected Transform playerListContainer;

        // List to keep track of instantiated player panels
        protected List<GameObject> instantiatedPlayerPanels = new List<GameObject>();

        private void OnEnable()
        {
            GameLobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            GameLobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        }

        private void OnLobbyUpdated()
        {
            List<LobbyPlayerData> playerDataList = GameLobbyManager.Instance.GetPlayerDataList();
            RestorePlayers();

            for (int i = 0; i < playerDataList.Count; i++)
            {
                LobbyPlayerData playerData = playerDataList[i];
                UpdateAction(playerData);
            }
        }

        private void RestorePlayers()
        {
            foreach (var panel in instantiatedPlayerPanels)
            {
                Destroy(panel);
            }
            instantiatedPlayerPanels.Clear();
        }

        protected abstract void UpdateAction(LobbyPlayerData playerData);
    }
}
