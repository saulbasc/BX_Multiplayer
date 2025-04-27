using System.Collections;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game.Manager
{
    public class GameStatusManager : NetworkBehaviour
    {
        [SerializeField] private MatchStateManager matchStateManager;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                matchStateManager.OnMatchStateChanged += HandleStateChanged;
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
            if (MatchInfo.Instance.GetAllConnected())
            {
                matchStateManager.SetMatchState(MatchState.starting);
            }
        }

        private void HandleStateChanged(MatchState state)
        {
            if(!IsServer) return;

            Debug.Log(state);
            if(state == MatchState.starting) StartCoroutine(startMatchCountdown(5));
            else if (state == MatchState.gameOver) SceneManager.LoadSceneAsync("GameOverScene");
            else if (state == MatchState.exit) SceneManager.LoadSceneAsync("MenuScene");
        }

        private IEnumerator startMatchCountdown(int countdownTime)
        {
            while (countdownTime > 0)
            {
                Debug.Log("Comenzando en " + countdownTime);
                yield return new WaitForSeconds(1);
                countdownTime--;
            }

            matchStateManager.SetMatchState(MatchState.playing);
        }
    }
}
