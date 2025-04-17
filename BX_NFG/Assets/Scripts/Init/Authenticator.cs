
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Init
{
    public class Authenticator : MonoBehaviour
    {
        async void Start()
        {
            await UnityServices.InitializeAsync();

            if(UnityServices.State == ServicesInitializationState.Initialized)
            {
                AuthenticationService.Instance.SignedIn += OnSignedIn;
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (AuthenticationService.Instance.IsSignedIn)
                {
                    string playerName = PlayerPrefs.GetString("Username");
                    if (playerName == "")
                    {
                        playerName = "player";
                        PlayerPrefs.SetString("Username", playerName);
                    }
                    await SceneManager.LoadSceneAsync("MenuScene");
                }
            }
        }

        private void OnSignedIn()
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
            Debug.Log("Token: " + AuthenticationService.Instance.AccessToken);
        }
    }
}
