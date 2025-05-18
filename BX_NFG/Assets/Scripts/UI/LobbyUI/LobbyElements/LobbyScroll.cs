using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Lobbi.UI.PlayerEntry;
using Assets.Scripts.Lobbi.Players;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Data;

namespace Assets.Scripts.Lobbi.UI.TeamScroll
{
    public class LobbyScroll : MonoBehaviour
    {
        [SerializeField] private PlayerTeam teamPlayersToAdd;
        /// <summary>
        /// El panel que representa una instancia de un jugador local. 
        /// </summary>
        [SerializeField] protected GameObject localPlayerPanelPrefab;
        /// <summary>
        /// El panel que representa una instancia de un jugador espectador. 
        /// </summary>
        [SerializeField] protected GameObject playerPanelPrefab;
        /// <summary>
        /// El panel que representa una instancia de un jugador visitante. 
        /// </summary>
        [SerializeField] protected Transform playerListContainer;

        protected List<GameObject> instantiatedPlayerPanels = new List<GameObject>();

        private void OnEnable()
        {
            LobbyEvents.Instance.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            LobbyEvents.Instance.OnLobbyUpdated -= OnLobbyUpdated;
        }

        private void OnLobbyUpdated()
        {
            List<LobbyPlayerData> playerDataList = LobbyPlayerManager.Instance.GetAllPlayersDataObject();
            instantiatedPlayerPanels.ForEach(playerPanel => Destroy(playerPanel));
            instantiatedPlayerPanels.Clear();
            playerDataList.ForEach(playerData => {
                Debug.Log($"PlayerData: {playerData.GameTag} - {playerData.PlayerTeam}");
                if (playerData.PlayerTeam == teamPlayersToAdd)
                {
                    SetUIPanelPlayer(playerData);
                }
            });
        }

        private void SetUIPanelPlayer(LobbyPlayerData playerData)
        {
            GameObject playerPanel = SetPlayerPanel(playerData);
            instantiatedPlayerPanels.Add(playerPanel);
            SetLobbyPlayerPanelInfo(playerPanel, playerData);
        }

        /// <summary>
        /// Genera un panel específico del host o del cliente.
        /// </summary>
        /// <param name="playerData">Los datos del jugador.</param>
        /// <returns>El panel como GameObject.</returns>
        private GameObject SetPlayerPanel(LobbyPlayerData playerData)
        {
            if (UnityServicesActions.GetCurrentUserID() == playerData.Id)
            {
                GameObject playerPanel = Instantiate(localPlayerPanelPrefab, playerListContainer);
                PlayerPanel entry = playerPanel.GetComponent<PlayerPanel>();
                return playerPanel;
            } 
            else
            {
                return Instantiate(playerPanelPrefab, playerListContainer);
            }
        }

        /// <summary>
        /// Establece los datos del jugador en el panel.
        /// </summary>
        /// <param name="playerPanel">El panel del jugador.</param>
        /// <param name="playerData">Los datos del jugador.</param>
        private void SetLobbyPlayerPanelInfo(GameObject playerPanel, LobbyPlayerData playerData)
        {
            PlayerPanelUI lobbyPlayer = playerPanel.GetComponent<PlayerPanelUI>();
            lobbyPlayer.SetPlayerNameAndStatus(playerData.GameTag, playerData.IsReady);
        }
    }
}