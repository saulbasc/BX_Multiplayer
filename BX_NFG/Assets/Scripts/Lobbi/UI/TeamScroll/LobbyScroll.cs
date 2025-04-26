
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Connection.Lobbi;
using Assets.Scripts.Lobbi.UI.PlayerEntry;
using Assets.Scripts.Lobbi.Players;

namespace Assets.Scripts.Lobbi.UI.TeamScroll
{
    public abstract class LobbyScroll : MonoBehaviour
    {
        [SerializeField] protected GameObject localPlayerPanelPrefab;
        [SerializeField] protected GameObject playerPanelPrefab;
        [SerializeField] protected Transform playerListContainer;

        protected List<GameObject> instantiatedPlayerPanels = new List<GameObject>();

        private void OnEnable(){ GameLobbyEvents.OnLobbyUpdated += OnLobbyUpdated; }

        private void OnDisable(){ GameLobbyEvents.OnLobbyUpdated -= OnLobbyUpdated; }

        protected abstract void UpdateAction(LobbyPlayerData playerData);

        private void OnLobbyUpdated()
        {
            List<LobbyPlayerData> playerDataList = GameLobbyManager.Instance.GetPlayerDataList();
            instantiatedPlayerPanels.ForEach(playerPanel => Destroy(playerPanel));
            instantiatedPlayerPanels.Clear();
            playerDataList.ForEach(playerData => UpdateAction(playerData));
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
                ? GenerateLocalPlayerPanel(playerData)
                : Instantiate(playerPanelPrefab, playerListContainer);
        }

        protected GameObject GenerateLocalPlayerPanel(LobbyPlayerData playerData)
        {
            GameObject playerPanel = Instantiate(localPlayerPanelPrefab, playerListContainer);
            PlayerPanel entry = playerPanel.GetComponent<PlayerPanel>();
            entry?.Inicialize(playerData);
            return playerPanel;
        }

        protected void SetLobbyPlayer(GameObject playerPanel, LobbyPlayerData playerData)
        {
            PlayerPanelUI lobbyPlayer = playerPanel.GetComponent<PlayerPanelUI>();
            lobbyPlayer.SetPlayerName(playerData.GameTag, playerData.IsReady);
        }
    }
}