using Firebase.Firestore;

namespace Assets.Scripts.Core.Models
{
    [FirestoreData]
    public class Match
    {
        [FirestoreProperty]
        public float MatchDuration { get; private set; }

        [FirestoreProperty]
        public Team LocalTeam { get; private set; }

        [FirestoreProperty]
        public Team VisitorTeam { get; private set; }

        public Match(float matchDuration, Team localTeam, Team visitorTeam)
        {
            MatchDuration = matchDuration;
            LocalTeam = localTeam;
            VisitorTeam = visitorTeam;
        }

        public Match(float matchDuration) 
        {
            MatchDuration = matchDuration;
            LocalTeam = new Team();
            VisitorTeam = new Team();
        }

        public Match() { }
    }
}
