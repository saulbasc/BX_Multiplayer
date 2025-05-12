
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Lobbi.Players;
using UnityEngine;

namespace Assets.Scripts.Lobbi.UI.PlayerEntry
{
    public class PlayerPanel : MonoBehaviour
    {
        protected LobbyPlayerData playerData;

        public void Inicialize(LobbyPlayerData data)
        {
            playerData = data;
        }

        protected async void ChangeTeam(PlayerTeam playerTeam)
        {
            await LobbyPlayerManager.Instance.SetPlayerTeamAsync(playerData, playerTeam);
        }
    }
}
