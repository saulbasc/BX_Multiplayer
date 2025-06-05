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
        [SerializeField] private MatchStateManager matchStateManager;
        private bool allConectedFirstTime;
        private bool gameEnded;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                matchStateManager.OnMatchStateChanged += HandleStateChanged;
                gameEnded = false;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                matchStateManager.OnMatchStateChanged -= HandleStateChanged;
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
            matchStateManager.SetMatchState(MatchState.starting);
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
                if (!gameEnded)
                {
                    GameOverActions();
                    gameEnded = true;
                }
            }
            else if (state == MatchState.exit)
            {
                ShutDownMatch();
            }
            else if(state == MatchState.onGoal)
            {
                StartCoroutine(OnGoal());
            }
        }

        private void ShutDownMatch()
        {
            NetworkManager.Singleton.Shutdown();
        }

        private async void GameOverActions()
        {
            if(!IsServer) return;

            PlayerInGame[] players = FindObjectsByType<PlayerInGame>(FindObjectsSortMode.None);

            foreach (var player in players)
            {
                if(player.PlayerId != null)
                {
                    await PlayerStatsDAO.Instance.Insert(player.PlayerId, player.GetStats());
                    await PlayerMatchSummaryDAO.Instance.Insert(player.PlayerId, player.GetSummary());
                    await RankingDAO.Instance.Insert(player.PlayerId, player.GetRankingStats());
                }
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

            matchStateManager.SetMatchState(MatchState.starting);
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
            matchStateManager.SetMatchState(MatchState.playing);
        }
    }
}
