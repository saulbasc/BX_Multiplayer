using Firebase.Firestore;

namespace Assets.Scripts.Core.Models
{
    [FirestoreData]
    public class PlayerStats
    {
        [FirestoreProperty] public int MatchesPlayed { get; set; }
        [FirestoreProperty] public int Goals { get; set; }
        [FirestoreProperty] public int Touches { get; set; }
        [FirestoreProperty] public float SecondsPlayed { get; set; }
    }
}
