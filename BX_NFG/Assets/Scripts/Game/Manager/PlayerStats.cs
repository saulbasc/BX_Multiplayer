namespace Assets.Scripts.Game.Manager
{
    public class PlayerStats
    {
        public ulong PlayerId { set; get; }
        public float SecondsPlayed { set; get; }
        public int Goals { set; get; }

        public PlayerStats(ulong playerId)
        {
            PlayerId = playerId;
            SecondsPlayed = 0f;
            Goals = 0;
        }
    }
}
