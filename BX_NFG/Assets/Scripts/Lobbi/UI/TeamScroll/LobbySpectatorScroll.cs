
using Assets.Scripts.Lobbi.Data;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Lobbi.UI.TeamScroll
{
    public class LobbySpectatorScroll : LobbyTeamScroll
    {
        protected override void UpdateAction(LobbyPlayerData playerData)
        {
            if (playerData.PlayerTeam == PlayerTeam.Spectator)
            {
                GameObject playerPanel = Instantiate(playerPanelPrefab, playerListContainer);
                var playerNameText = playerPanel.GetComponentInChildren<TextMeshProUGUI>();
                playerNameText.text = playerData.GameTag;
                instantiatedPlayerPanels.Add(playerPanel);
            }
        }
    }
}
