
using Assets.Scripts.Lobbi.Data;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Lobbi.UI.TeamScroll
{
    public class LobbyLocalScroll : LobbyTeamScroll
    {
        protected override void UpdateAction(LobbyPlayerData playerData)
        {
            if (playerData.PlayerTeam == PlayerTeam.Local)
            {
                GameObject playerPanel = Instantiate(playerPanelPrefab, playerListContainer);
                var playerNameText = playerPanel.GetComponentInChildren<TextMeshProUGUI>();
                playerNameText.text = playerData.GameTag;
                instantiatedPlayerPanels.Add(playerPanel);
            }
        }
    }
}
