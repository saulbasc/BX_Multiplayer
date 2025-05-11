
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Players;

namespace Assets.Scripts.Lobbi.UI.TeamScroll
{
    public class LobbySpectatorScroll : LobbyScroll
    {
        protected override void UpdateAction(LobbyPlayerData playerData)
        {
            if (playerData.PlayerTeam == PlayerTeam.Spectator)
            {
                SetUIPlayer(playerData);
            }
        }
    }
}
