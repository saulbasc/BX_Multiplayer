using System.Collections;
using Assets.Scripts.Commons;
using Assets.Scripts.Core.Daos;
using Assets.Scripts.Game.GameEvents.Player;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game.Manager
{
    public class GameStatusManager : NetworkBehaviour
    {
        private bool allConectedFirstTime;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                MatchStateManager.Instance.OnMatchStateChanged += HandleStateChanged;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                MatchStateManager.Instance.OnMatchStateChanged -= HandleStateChanged;
            }
        }

        private void Update()
        {
            if (!IsServer) return;

            if (MatchInfo.Instance.GetAllConnected() && !allConectedFirstTime)
            {
                allConectedFirstTime = true;
                StartCoroutine(HandleAllConnected());
            }
        }

        private IEnumerator HandleAllConnected()
        {
            yield return new WaitForSeconds(5);
            MatchStateManager.Instance.SetMatchState(MatchState.starting);
            Debug.Log("CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC");
        }

        private void HandleStateChanged(MatchState state)
        {
            if(!IsServer) return;

            Debug.Log(state);
            if (state == MatchState.starting)
            {
                StartCoroutine(StartMatchCountdown(5));
            }
            else if (state == MatchState.gameOver)
            {
                GameOverActions();
            }
            else if (state == MatchState.exit)
            {
                SetGameMenuSceneRpc();
            }
            else if(state == MatchState.onGoal)
            {
                StartCoroutine(OnGoal());
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void SetGameMenuSceneRpc()
        {
            SceneManager.LoadSceneAsync(Scenes.MenuScene.ToString());
        }

        private async void GameOverActions()
        {
            if(!IsServer) return;

            PlayerInGame[] players = FindObjectsByType<PlayerInGame>(FindObjectsSortMode.None);

            foreach (var player in players)
            {
                await PlayerStatsDAO.Instance.Insert(player.PlayerId, player.GetStats());
            }
        }

        private IEnumerator OnGoal()
        {
            int countdownTime = 5;
            while(countdownTime > 0)
            {
                Debug.Log("Dejando de celebrar en => " + countdownTime);
                yield return new WaitForSeconds(1);
                countdownTime--;
            }

            MatchStateManager.Instance.SetMatchState(MatchState.starting);
        }

        public delegate void UpdateSecondsLeft(int secondsLeft);
        public static UpdateSecondsLeft OnUpdateSecondsLeft;

        private IEnumerator StartMatchCountdown(int countdownTime)
        {
            while (countdownTime > 0)
            {
                yield return new WaitForSeconds(1);
                countdownTime--;
                OnUpdateSecondsLeft?.Invoke(countdownTime);
            }

            OnUpdateSecondsLeft?.Invoke(0);
            MatchStateManager.Instance.SetMatchState(MatchState.playing);
        }
    }
}
