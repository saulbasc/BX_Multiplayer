
namespace Assets.Scripts.Init
{
    public static class InitEvents
    {
        public delegate void UnityServicesSignIn();
        public static UnityServicesSignIn OnUnityServicesSignIn;

        public delegate void FirebaseSignIn();
        public static FirebaseSignIn OnFirebaseSignIn;
    }
}
