

using Firebase;
using Firebase.Extensions;
using UnityEngine;

namespace Assets.Scripts.Core.FireB
{
    public class FirebaseInit : MonoBehaviour
    {
        private void Start()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    Debug.Log("Firebase listo para usar.");
                    Login();
                }
                else
                {
                    Debug.LogError($"No se pudieron resolver todas las dependencias de Firebase: {dependencyStatus}");
                }
            });
        }

        private void Login()
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

            if(auth.CurrentUser != null)
            {
                Debug.Log("Ya hay un usuario autenticado.");
                return;
            }

            auth.SignInAnonymouslyAsync().ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInAnonymouslyAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                    return;
                }

                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
            });
        }
    }
}

