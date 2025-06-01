using Firebase.Firestore;

namespace Assets.Scripts.Core.Models
{
    [FirestoreData]
    public class GamePlayer
    {
        public ulong PlayerGameId {  get; set; }

        [FirestoreProperty]
        public string PlayerId { get; set; }

        [FirestoreProperty]
        public int SecondsPlayed { get; set; }

        [FirestoreProperty]
        public int Touches { get; set; }

        [FirestoreProperty]
        public int Goals { get; set; }

        public GamePlayer(ulong playerGameId, string playerId, int secondsPlayed, int touches, int goals)
        {
            PlayerGameId = playerGameId;
            PlayerId = playerId;
            SecondsPlayed = secondsPlayed;
            Touches = touches;
            Goals = goals;
        }

        public GamePlayer(ulong playerGameId, string playerId)
        {
            PlayerGameId = playerGameId;
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
