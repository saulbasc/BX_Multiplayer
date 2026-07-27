using Firebase.Firestore;

namespace Assets.Scripts.Core.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string FirebaseId { get; private set; }
        [FirestoreProperty]
        public string AuthUnityId { get; private set; }
        [FirestoreProperty]
        public string Username { get; private set; }

        public User(string firebaseId, string authUnityId, string username)
        {
            FirebaseId = firebaseId;
            AuthUnityId = authUnityId;
            Username = username;
        }

        public User(string firebaseId, string username)
        {
            FirebaseId = firebaseId;
            Username = username;
        }

        public User() { }

        public void SetUsername(string username)
        {
            Username = username;
        }
    }
}