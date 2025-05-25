using System.Collections.Generic;
using Assets.Scripts.Commons;

namespace Assets.Scripts.Game.Manager
{
    public class MatchStats : NetworkSingleton<MatchStats>
    {
        public int LocalScore { get; set; }
        public int VisitorScore { get; set; }
        public List<PlayerStats> PlayerStatsList { get; private set; }

        public void AddPlayer(ulong playerId)
        {
            PlayerStatsList.Add(new PlayerStats(playerId));
        }

        public void RemovePlayer(ulong playerId)
        {
            PlayerStatsList.RemoveAll(player => player.PlayerId == playerId);
        }
    }
}
