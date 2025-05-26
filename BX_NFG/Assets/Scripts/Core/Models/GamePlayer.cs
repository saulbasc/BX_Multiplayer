using Firebase.Firestore;

namespace Assets.Scripts.Core.Models
{
    [FirestoreData]
    public class GamePlayer
    {
        [FirestoreProperty]
        public string PlayerId { get; set; }

        [FirestoreProperty]
        public int SecondsPlayed { get; set; }

        [FirestoreProperty]
        public int Touches { get; set; }

        [FirestoreProperty]
        public int Goals { get; set; }

        public GamePlayer(string playerId, int secondsPlayed, int touches, int goals)
        {
            PlayerId = playerId;
            SecondsPlayed = secondsPlayed;
            Touches = touches;
            Goals = goals;
        }

        public GamePlayer(string playerId)
        {
            PlayerId = playerId;
            SecondsPlayed = 0;
            Touches = 0;
            Goals = 0;
        }

        public GamePlayer() { }

        public void AddGoal()
        {
            Goals++;
        }

        public void AddTouch()
        {
            Touches++;
        }

        public void SetSeccondsPlayed(int seconds)
        {
            SecondsPlayed = seconds;
        }
    }
}
