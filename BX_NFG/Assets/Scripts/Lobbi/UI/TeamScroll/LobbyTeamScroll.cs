
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Connection.Lobbi;
using Assets.Scripts.Lobbi.UI.PlayerEntry;
using Assets.Scripts.Lobbi.Players;

namespace Assets.Scripts.Lobbi.UI.TeamScroll
{
    public abstract class LobbyTeamScroll : MonoBehaviour
    {
        [SerializeField] protected GameObject hostPlayerPanelPrefab;
        [SerializeField] protected GameObject playerPanelPrefab;
        [SerializeField] protected Transform playerListContainer;

        protected List<GameObject> instantiatedPlayerPanels = new List<GameObject>();

        private void OnEnable(){ GameLobbyEvents.OnLobbyUpdated += OnLobbyUpdated; }

        private void OnDisable(){ GameLobbyEvents.OnLobbyUpdated -= OnLobbyUpdated; }

        private void OnLobbyUpdated()
        {
            List<LobbyPlayerData> playerDataList = GameLobbyManager.Instance.GetPlayerDataList();
            RestorePlayers();
            playerDataList.ForEach(playerData => UpdateAction(playerData));
        }

        private void RestorePlayers()
        {
            instantiatedPlayerPanels.ForEach (panel => Destroy(panel));
            instantiatedPlayerPanels.Clear();
        }

        protected void SetUIPlayer(LobbyPlayerData playerData)
        {
            GameObject playerPanel = SetPlayerPanel(playerData);
            instantiatedPlayerPanels.Add(playerPanel);
            SetLobbyPlayer(playerPanel, playerData);
        }

        protected GameObject SetPlayerPanel(LobbyPlayerData playerData)
        {
            return GameLobbyManager.Instance.GetLocalID() == playerData.Id
                ? GenerateHostPlayerPanel(playerData)
                : GenerateClientPlayerPanel(playerData);
        }

        protected GameObject GenerateHostPlayerPanel(LobbyPlayerData playerData)
        {
            GameObject playerPanel = Instantiate(hostPlayerPanelPrefab, playerListContainer);
            PlayerEntryHost entry = playerPanel.GetComponent<PlayerEntryHost>();
            if (entry != null) { entry.SetPlayerData(playerData); }
            return playerPanel;
        }

        protected GameObject GenerateClientPlayerPanel(LobbyPlayerData playerData)
        {
            return Instantiate(playerPanelPrefab, playerListContainer);
        }

        protected void SetLobbyPlayer(GameObject playerPanel, LobbyPlayerData playerData)
        {
            LobbyPlayerUI lobbyPlayer = playerPanel.GetComponent<LobbyPlayerUI>();
            lobbyPlayer.SetPlayerName(playerData.GameTag, playerData.IsReady);
        }

        protected abstract void UpdateAction(LobbyPlayerData playerData);
    }
}