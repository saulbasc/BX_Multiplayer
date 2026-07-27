namespace Assets.Scripts.GameManager.GameEvents.Timer
{
    public enum MatchDuration
    {
        matchDuration1,
        matchDuration2,
        matchDuration3,
        matchDuration4,
        matchDuration5,
    }

    public static class MatchDurationExtensions
    {
        public static string ToString(MatchDuration duration)
        {
            return duration switch
            {
                MatchDuration.matchDuration1 => "1 min",
                MatchDuration.matchDuration2 => "2 min",
                MatchDuration.matchDuration3 => "3 min",
                MatchDuration.matchDuration4 => "4 min",
                MatchDuration.matchDuration5 => "5 min",
                _ => throw new System.NotImplementedException(),
            };
        }

        public static int ToInt(MatchDuration duration)
        {
            return duration switch
            {
                MatchDuration.matchDuration1 => 60,
                MatchDuration.matchDuration2 => 120,
                MatchDuration.matchDuration3 => 180,
                MatchDuration.matchDuration4 => 240,
                MatchDuration.matchDuration5 => 300,
                _ => throw new System.NotImplementedException(),
            };
        }

        public static float ToFloat(MatchDuration duration)
        {
            return duration switch
            {
                MatchDuration.matchDuration1 => 60f,
                MatchDuration.matchDuration2 => 120f,
                MatchDuration.matchDuration3 => 180f,
                MatchDuration.matchDuration4 => 240f,
                MatchDuration.matchDuration5 => 300f,
                _ => throw new System.NotImplementedException(),
            };
        }

        public static MatchDuration[] MatchDurationList()
        {
            return new MatchDuration[]
            {
                MatchDuration.matchDuration1,
                MatchDuration.matchDuration2,
                MatchDuration.matchDuration3,
                MatchDuration.matchDuration4,
                MatchDuration.matchDuration5
            };
        }
    }
}
