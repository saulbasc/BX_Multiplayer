using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Assets.Scripts.Core.FireB;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Handlers;
using Assets.Scripts.UI.InitUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Init
{
    /// <summary>
    /// Clase encargada de gestionar la autenticación del usuario en Firebase y Unity al entrar en la aplicación.
    /// </summary>
    public class Authenticator : MonoBehaviour
    {
        private void Awake()
        {
            InitUIEvents.Instance.onConfirmButtonClicked += RegisterNewUserInFirebase;
            AuthenticationProcess();
        }

        private void OnDestroy()
        {
            InitUIEvents.Instance.onConfirmButtonClicked -= RegisterNewUserInFirebase;
        }

        /// <summary>
        /// Determina el proceso inicial del juego dependiendo del registro del usuario.
        /// Si el usuario es válido continuará directamente a la pantalla del menú.
        /// </summary>
        private async void AuthenticationProcess()
        {
            await StartAuthenticationInitialize();
            if (AreLocalUserIDs())
            {
                if (await CheckUserRegisteredInFirebase())
                {
                    await SceneManager.LoadSceneAsync(Scenes.MenuScene.ToString());
                }
                else
                {
                    FirebaseActions.SignOutLocal();
                    await FirebaseActions.CreateNewFirebaseUser();
                    InitEventManager.Instance.RaiseUserNotRegistered();
                }
            }
            else
            {
                await FirebaseActions.CreateNewFirebaseUser();
                InitEventManager.Instance.RaiseUserNotRegistered();
            }
        }

        /// <summary>
        /// Inicia el proceso de autenticación del usuario con Firebase y UnityServices
        /// </summary>
        private async Task StartAuthenticationInitialize()
        {
            await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                await FirebaseActions.InitializeFirebaseForUser();
                await UnityServicesActions.InicializeUnityServicesForUser();
            });
        }

        /// <summary>
        /// Comprueba si hay un id de firebase y unityServices guardados en el dispositivo local 
        /// </summary>
        /// <returns>True si hay un id de unityServices y de Firebase en local</returns>
        private bool AreLocalUserIDs()
        {
            string firebaseID = FirebaseActions.GetCurrentID();
            string unityServicesID = UnityServicesActions.GetCurrentUserID();
            return !string.IsNullOrEmpty(firebaseID) && !string.IsNullOrEmpty(unityServicesID);
        }

        /// <summary>
        /// Comprueba si el usuario ya está registrado en Firebase.
        /// </summary>
        private async Task<bool> CheckUserRegisteredInFirebase()
        {
            string firebaseId = FirebaseActions.GetCurrentID();
            string unityId = UnityServicesActions.GetCurrentUserID();

            return await SafeAsyncFunctionsHandler.ExecuteAsync<bool>(async () =>
            {
                return await UserDAO.Instance.exists(firebaseId, unityId);
            });
        }

        /// <summary>
        /// Registra a un nuevo usuario en firebase.
        /// También instancia la escena inicial del menú del juego.
        /// </summary>
        private async void RegisterNewUserInFirebase(string userName)
        {
            if (IsValidUserName(userName))
            {
                await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
                {
                    bool success = await UserDAO.Instance.insert(GenerateNewDefaultUser(userName));

                    if (success)
                    {
                        InitEventManager.Instance.RaiseUserRegisteredSuccessfully();
                        await SceneManager.LoadSceneAsync(Scenes.MenuScene.ToString());
                    }
                });
            } 
        }

        /// <summary>
        /// Genera un nuevo objeto usuario con los ids por defecto.
        /// </summary>
        /// <param name="userName">El nombre del usuario</param>
        /// <returns>El objeto usuario generado</returns>
        private User GenerateNewDefaultUser(string userName)
        {
            string firebaseId = FirebaseActions.GetCurrentID();
            string unityId = UnityServicesActions.GetCurrentUserID();
            return new User(firebaseId, unityId, userName);
        }

        /// <summary>
        /// Comprueba si el nombre del usuario introducido cumple los requisitos
        /// </summary>
        /// <param name="username">Nombre introducido por el usuario</param>
        /// <returns></returns>
        private bool IsValidUserName(string username)
        {
            return !string.IsNullOrEmpty(username) && username.Length >= 3 && username.Length <= 20;
        }
    }
}
