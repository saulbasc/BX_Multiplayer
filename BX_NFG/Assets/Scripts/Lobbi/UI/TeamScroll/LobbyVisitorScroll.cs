
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Players;

namespace Assets.Scripts.Lobbi.UI.TeamScroll
{
    public class LobbyVisitorScroll : LobbyScroll
    {
        protected override void UpdateAction(LobbyPlayerData playerData)
        {
            if (playerData.PlayerTeam == PlayerTeam.Visitor)
            {
                SetUIPlayer(playerData);
            }
        }
    }
}
