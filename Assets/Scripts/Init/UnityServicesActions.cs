using System.Threading.Tasks;
using Assets.Scripts.Handlers;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Assets.Scripts.Init
{
    /// <summary>
    /// Clase que maneja la inicialización con UnityServices y los datos de autenticación.
    /// </summary>
    public static class UnityServicesActions
    {
        /// <summary>
        /// Inicializa los servicios de Unity si no están inicializados y realiza el inicio de sesión anónimo.
        /// También registra los eventos de autenticación.
        /// </summary>
        public static async Task InicializeUnityServicesForUser()
        {
            await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                if (UnityServices.State != ServicesInitializationState.Initialized)
                {
                    await UnityServices.InitializeAsync();
                }

                if (!AuthenticationService.Instance.IsSignedIn && !AuthenticationService.Instance.IsAuthorized)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
            });

            Debug.Log("Current user id => " +GetCurrentUserID());
        }

        /// <summary>
        /// Devuelve el ID del usuario autenticado actualmente en UnityServices.
        /// </summary>
        /// <returns>ID del jugador autenticado.</returns>
        public static string GetCurrentUserID()
        {
            return AuthenticationService.Instance.PlayerId;
        }

        /// <summary>
        /// Cierra la sesión del usuario en unityServices
        /// </summary>
        public static void SignOut() 
        {
            AuthenticationService.Instance.SignOut();
        }
    }
}
