using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Assets.Scripts.Init
{
    public static class UnityServicesActions
    {
        private static bool eventsRegistred = false;

        public static async Task Init()
        {
            try
            {
                if (Unity.Services.Core.UnityServices.State != ServicesInitializationState.Initialized)
                {
                    await Unity.Services.Core.UnityServices.InitializeAsync();
                }

                RegisterAuthEvents(); 

                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch (Exception e)
            {
                Debug.LogError("Error inicializando Unity Services: " + e.Message);
            }
        }

        private static void RegisterAuthEvents()
        {
            if (eventsRegistred) return;

            AuthenticationService.Instance.SignedIn += () =>
            {
                InitEvents.OnUnityServicesSignIn?.Invoke();
            };

            AuthenticationService.Instance.SignedOut += () =>
            {
                Debug.Log("[Auth] Usuario desconectado");
            };

            AuthenticationService.Instance.Expired += () =>
            {
                Debug.LogWarning("[Auth] La sesión ha expirado. Se requiere nueva autenticación.");
            };

            AuthenticationService.Instance.SignInFailed += error =>
            {
                Debug.LogError("[Auth] Falló el login: " + error);
            };

            eventsRegistred = true;
        }

        public static string GetCurrentID()
        {
            return AuthenticationService.Instance.PlayerId;
        }
    }
}
