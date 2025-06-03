using Assets.Scripts.UI.MenuUI.Components;
using Firebase.Firestore;

namespace Assets.Scripts.Core.Models
{
    [FirestoreData]
    public class PlayerMatchSummary
    {
        [FirestoreProperty] public int LocalScore { get; set; }
        [FirestoreProperty] public int VisitorScore { get; set; }
        [FirestoreProperty] public MatchResult Result { get; set; }
    }
}
