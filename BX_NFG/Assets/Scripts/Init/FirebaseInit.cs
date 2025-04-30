using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.FireB
{
    public static class FirebaseInit
    {
        public static async Task<bool> Init()
        {
            var result = await InitializeFirebaseAsync();
            if (result)
            {
                return true;
            }
            else
            {
                Debug.LogError("Falló la inicialización o el login de firebase.");
                return false;
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
            return await LoginAsync();
        }

        private static async Task<bool> LoginAsync()
        {
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;

            if (auth.CurrentUser != null)
            {
                Debug.Log("Ya hay un usuario autenticado en firebase => " + FirebaseAuth.DefaultInstance.CurrentUser.UserId);
                return true;
            }

            try
            {
                var result = await auth.SignInAnonymouslyAsync();
                Debug.LogFormat("Usuario registrado con éxito: {0}", result.User.UserId);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al iniciar sesión anónimamente: " + e);
                return false;
            }
        }

        public static string GetCurrentID()
        {
            return FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        }
    }
}
