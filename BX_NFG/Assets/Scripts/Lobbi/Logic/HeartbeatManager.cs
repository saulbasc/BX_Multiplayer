
using System;
using System.Collections;
using Assets.Scripts.Commons;
using Unity.Services.Lobbies;
using UnityEngine;

namespace Assets.Scripts.Lobbi.Logic
{
    public class HeartbeatManager : Singleton<HeartbeatManager>
    {
        private Coroutine lobbyCoroutine;

        public void StartUpdating(string lobbyId, float interval)
        {
            if (lobbyCoroutine == null)
            {
                lobbyCoroutine = StartCoroutine(RefreshCoroutine(lobbyId, interval));
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

        private IEnumerator RefreshCoroutine(string lobbyId, float wait)
        {
            while (true)
            {
                try
                {
                    LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
                yield return new WaitForSecondsRealtime(wait);
            }
        }
    }
}
