using System.Collections;
using Assets.Scripts.Commons;
using Assets.Scripts.Lobbi.Logic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Lobbi.LobbyManager
{
    public class LobbyStarter : MonoBehaviour
    {
        [SerializeField] private LobbyServiceManager lobbyServiceManager;
        private LobbyIntent lobbyIntent;
        private void Awake()
        {
            StartCoroutine(SetLobbyStarter());
        }

        private IEnumerator SetLobbyStarter()
        {
            while (lobbyIntent == null)
            {
                lobbyIntent = LobbyIntent.Instance;
                yield return null;
            }

            LobbyCheck();
        }

        private async void LobbyCheck()
        {
            if (lobbyIntent.IsCreatingLobby)
            {
                await lobbyServiceManager.CreateLobby();
                LobbyEvents.Instance.RaiserLobbyReadyToStart();
            }
            else
            {
                bool success = await lobbyServiceManager.JoinLobby(lobbyIntent.JoinCode);
                if (success)
                {
                    LobbyEvents.Instance.RaiserLobbyReadyToStart();
                }
                else
                {
                    SceneManager.LoadScene(Scenes.MenuScene.ToString());
                }
            }
        }
    }
}
