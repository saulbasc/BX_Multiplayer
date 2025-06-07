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

        public static int ToInt(MatchDuration duration)
        {
            return duration switch
            {
                MatchDuration.matchDuration1 => 30,
                MatchDuration.matchDuration3 => 180,
                MatchDuration.matchDuration5 => 300,
                MatchDuration.matchDuration7 => 420,
                MatchDuration.matchDuration10 => 600,
                _ => throw new System.NotImplementedException(),
            };
        }

        public static float ToFloat(MatchDuration duration)
        {
            return duration switch
            {
                MatchDuration.matchDuration1 => 30f,
                MatchDuration.matchDuration3 => 180f,
                MatchDuration.matchDuration5 => 300f,
                MatchDuration.matchDuration7 => 420f,
                MatchDuration.matchDuration10 => 600f,
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
