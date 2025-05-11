using System.Collections.Generic;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Lobbi.Players;
using Unity.Services.Lobbies.Models;

namespace Assets.Scripts.Lobbi.Util
{
    public static class LobbyUtil
    {
        public static int NumberOfPlayersReady(List<Dictionary<string, PlayerDataObject>> players)
        {
            int numberOfPlayersReady = 0;
            players.ForEach(player => { numberOfPlayersReady = SumPlayerReady(player, numberOfPlayersReady); });
            return numberOfPlayersReady;
        }

        public static int SumPlayerReady(Dictionary<string, PlayerDataObject> playerData, int playersReady)
        {
            LobbyPlayerData player = new LobbyPlayerData(playerData);
            return player.IsReady
                ? playersReady + 1
                : playersReady;
        }
    }
}
