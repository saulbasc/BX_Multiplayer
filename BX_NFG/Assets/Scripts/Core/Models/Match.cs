using Firebase.Firestore;
using Unity.Netcode;

namespace Assets.Scripts.Core.Models
{
    public class Match
    {
        private float matchDuration;
        private int localScore;
        private int visitorScore;

        public float MatchDuration => matchDuration;
        public int LocalScore => localScore;
        public int VisitorScore => visitorScore;

        public void AddLocalGoal() => localScore++;
        public void AddVisitorGoal() => visitorScore++;

        public Match(float matchDuration)
        {
            this.matchDuration = matchDuration;
            localScore = 0;
            visitorScore = 0;
        }

        [ServerRpc]
        public Match GetMatchServerRpc()
        {
            return this;
        }
    }
}
