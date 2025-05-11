using System.Threading.Tasks;
using System;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Commons;
using Assets.Scripts.Lobbi.Util;
using System.Collections.Generic;
using Assets.Scripts.Relay;
using UnityEngine.SceneManagement;
using WebSocketSharp;

namespace Assets.Scripts.Lobbi.Logic
{
    public class LobbyUpdaterManager : Singleton<LobbyUpdaterManager>
    {
        private Coroutine lobbyCoroutine;

        private void OnEnable()
        {
            LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
            Destroy(gameObject);
        }

        public void StartUpdating(string lobbyId, float interval)
        {
            if (lobbyCoroutine == null)
            {
                lobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(lobbyId, interval));
            }
        }

        public void StopUpdating()
        {
            if (lobbyCoroutine != null)
            {
                StopCoroutine(lobbyCoroutine);
                lobbyCoroutine = null;
            }
        }

        private void OnDestroy()
        {
            StopUpdating();
        }

        private IEnumerator RefreshLobbyCoroutine(string lobbyId, float wait)
        {
            while (true)
            {
                yield return TryUpdateLobby(lobbyId);
                yield return new WaitForSecondsRealtime(wait);
            }
        }

        private IEnumerator TryUpdateLobby(string lobbyId)
        {
            Task<Lobby> task = null;

            try
            {
                task = LobbyService.Instance.GetLobbyAsync(lobbyId);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                yield break;
            }

            yield return new WaitUntil(() => task.IsCompleted);

            if (task.IsCompletedSuccessfully)
            {
                HandleLobbyResult(task.Result);
            }
            else
            {
                Debug.LogError($"Error al obtener el lobby: {task.Exception?.Flatten().InnerException}");
            }
        }

        private void HandleLobbyResult(Lobby newLobby)
        {
            try
            {
                if (newLobby.LastUpdated > LobbyDataManager.Instance.Lobby.LastUpdated)
                {
                    LobbyEvents.OnLobbyUpdated?.Invoke(newLobby);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async void OnLobbyUpdated(Lobby lobby)
        {
            List<Dictionary<string, PlayerDataObject>> players = LobbyPlayersManager.Instance.GetAllPlayersData();

            GameLobbyEvents.OnLobbyUpdated?.Invoke();

            if (LobbyUtil.NumberOfPlayersReady(players) == players.Count)
            {
                GameLobbyEvents.OnLobbyReady?.Invoke();
            }
            else
            {
                GameLobbyEvents.OnLobbyCancel?.Invoke();
            }

            if (!LobbyDataManager.Instance.GetLobbyDataObject().RelayJoinCode.IsNullOrEmpty() && !PlayerStatus.Instance.InGame && !PlayerStatus.Instance.JoinedGame)
            {
                Debug.Log("ERRRR?");
                try
                {
                    await ClientRelayManager.Instance.JoinRelayServer();
                    await SceneManager.LoadSceneAsync(Scenes.GameScene.ToString());
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                }
                PlayerStatus.Instance.JoinedGame = true;
            }
        }

        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}
