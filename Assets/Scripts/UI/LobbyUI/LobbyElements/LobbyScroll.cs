using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Lobbi.UI.PlayerEntry;
using Assets.Scripts.Lobbi.Players;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Data;
using System.Collections;

namespace Assets.Scripts.Lobbi.UI.TeamScroll
{
    public class LobbyScroll : MonoBehaviour
    {
        private LobbyPlayerManager lobbyPlayerManager;
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

        private bool canStart;

        protected List<GameObject> instantiatedPlayerPanels = new List<GameObject>();

        private void OnEnable()
        {
            LobbyEvents.Instance.OnLobbyReadyToStart += CanStart;
            LobbyEvents.Instance.OnLobbyUpdated += OnLobbyUpdated;
            StartCoroutine(SetLobbyScroll());
        }

        private void OnDisable()
        {
            LobbyEvents.Instance.OnLobbyReadyToStart -= CanStart;
            LobbyEvents.Instance.OnLobbyUpdated -= OnLobbyUpdated;
        }

        private IEnumerator SetLobbyScroll()
        {
            while (lobbyPlayerManager == null)
            {
                lobbyPlayerManager = FindFirstObjectByType<LobbyPlayerManager>();
                if (lobbyPlayerManager == null)
                {
                    yield return null;
                }
            }
        }

        private void OnLobbyUpdated()
        {
            if (!canStart || lobbyPlayerManager == null) return;

            List<LobbyPlayerData> playerDataList = lobbyPlayerManager.GetAllPlayersDataObject();
            instantiatedPlayerPanels.ForEach(playerPanel => Destroy(playerPanel));
            instantiatedPlayerPanels.Clear();
            playerDataList.ForEach(playerData => {
                if (playerData.PlayerTeam == teamPlayersToAdd)
                {
                    SetUIPanelPlayer(playerData);
                }
            });
        }

        private void CanStart()
        {
            canStart = true;
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