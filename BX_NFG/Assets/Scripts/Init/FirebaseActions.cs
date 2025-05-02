using Assets.Scripts.Init;
using Firebase;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.FireB
{
    public static class FirebaseActions
    {
        private static bool eventsRegistered = false;

        public static async Task Init()
        {
            var result = await InitializeFirebaseAsync();
            if (!result)
            {
                Debug.LogError("Falló la inicialización o el login de Firebase.");
            }
        }

        private static async Task<bool> InitializeFirebaseAsync()
        {
            var dependencyResult = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyResult != DependencyStatus.Available)
            {
                Debug.LogError($"No se pudieron resolver todas las dependencias de Firebase: {dependencyResult}");
                return false;
            }

            RegisterAuthEvents();
            return await LoginAsync();
        }

        private static async Task<bool> LoginAsync()
        {
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;

            if (auth.CurrentUser != null)
            {
                InitEvents.OnFirebaseSignIn?.Invoke();
                return true;
            }

            try
            {
                var result = await auth.SignInAnonymouslyAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Error al iniciar sesión anónimamente: " + e);
                return false;
            }
        }

        private static void RegisterAuthEvents()
        {
            if (eventsRegistered) return;

            FirebaseAuth.DefaultInstance.StateChanged += (sender, args) =>
            {
                var auth = FirebaseAuth.DefaultInstance;
                if (auth.CurrentUser != null)
                {
                    Debug.Log("[Firebase] Usuario conectado: " + auth.CurrentUser.UserId);
                    InitEvents.OnFirebaseSignIn?.Invoke();
                }
            };

            eventsRegistered = true;
        }

        public static string GetCurrentID()
        {
            return FirebaseAuth.DefaultInstance.CurrentUser?.UserId ?? "null";
        }
    }
}
