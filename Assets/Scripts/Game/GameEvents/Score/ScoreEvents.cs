
namespace Assets.Scripts.Game.GameEvents.Score
{
    public static class ScoreEvents
    {
        public delegate void LocalGoalScored();
        public static LocalGoalScored OnLocalGoalScored;

        public delegate void VisitorGoalScored();
        public static VisitorGoalScored OnVisitorGoalScored;

        public delegate void UpdateLocalGoalScored(int goals);
        public static UpdateLocalGoalScored OnUpdateLocalGoalScored;

        public delegate void UpdateVisitorGoalScored(int goals);
        public static UpdateVisitorGoalScored OnUpdateVisitorGoalScored;
    }
}
