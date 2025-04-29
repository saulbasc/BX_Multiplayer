using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Assets.Scripts.Init
{
    public static class UnityServicesInit
    {
        public static async Task<bool> Init()
        {
            await UnityServices.InitializeAsync();

            if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                AuthenticationService.Instance.SignedIn += OnSignedIn;
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (AuthenticationService.Instance.IsSignedIn)
                {
                    return true;
                }
            }

            return false;
        }

        private static void OnSignedIn()
        {
            Debug.Log("Id del usuario de AuthService de unity => " + AuthenticationService.Instance.PlayerId);
        }

        public static string GetCurrentID()
        {
            return AuthenticationService.Instance.PlayerId;
        }
    }
}
