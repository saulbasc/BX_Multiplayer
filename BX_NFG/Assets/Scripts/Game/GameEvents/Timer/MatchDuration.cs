namespace Assets.Scripts.GameManager.GameEvents.Timer
{
    public enum MatchDuration
    {
        matchDuration1,
        matchDuration3,
        matchDuration5,
        matchDuration7,
        matchDuration10,
    }

    public static class MatchDurationExtensions
    {
        public static string ToString(MatchDuration duration)
        {
            return duration switch
            {
                MatchDuration.matchDuration1 => "1 min",
                MatchDuration.matchDuration3 => "3 min",
                MatchDuration.matchDuration5 => "5 min",
                MatchDuration.matchDuration7 => "7 min",
                MatchDuration.matchDuration10 => "10 min",
                _ => throw new System.NotImplementedException(),
            };
        }

        public static MatchDuration[] MatchDurationList()
        {
            return new MatchDuration[]
            {
                MatchDuration.matchDuration1,
                MatchDuration.matchDuration3,
                MatchDuration.matchDuration5,
                MatchDuration.matchDuration7,
                MatchDuration.matchDuration10
            };
        }
    }
}
