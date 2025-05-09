using Assets.Scripts.Handlers;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.FireB
{
    /// <summary>
    /// Maneja la inicialización con firebase y los datos de autenticación.
    /// </summary>
    public static class FirebaseActions
    {
        /// <summary>
        /// Comprueba que todas las dependencias de firebase estén correctas antes de proceder al registro.
        /// </summary>
        /// <returns></returns>
        public static async Task InitializeFirebaseForUser()
        {
            var dependencyResult = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyResult != DependencyStatus.Available)
            {
                Debug.LogError($"No se pudieron resolver todas las dependencias de Firebase: {dependencyResult}");
            }
        }

        /// <summary>
        /// Crea un nuevo usuario anónimo en el Authenticator de Firebase y lo guarda en local.
        /// </summary>
        public static async Task CreateNewFirebaseUser()
        {
            await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                await FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync();
                Debug.Log("[Firebase] Usuario conectado: " + GetCurrentID());
            });
        }

        /// <summary>
        /// Obtener el ID de autenticación de firebase del usuario.
        /// </summary>
        /// <returns>El ID de firebase del usuario</returns>
        public static string GetCurrentID()
        {
            Debug.Log("Current user firebase "+FirebaseAuth.DefaultInstance.CurrentUser?.UserId);
            return FirebaseAuth.DefaultInstance.CurrentUser?.UserId ?? null;
        }

        /// <summary>
        /// Elimina el id de firebase asociado al dispositivo
        /// </summary>
        public static void SignOutLocal()
        {
            FirebaseAuth.DefaultInstance.SignOut();
        }
    }
}