using System.Collections;
using Assets.Scripts.Commons;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game.Manager
{
    public class GameStatusManager : NetworkBehaviour
    {
        [SerializeField] private MatchStateManager matchStateManager;
        private Coroutine startingCoroutine;
        private bool allConectedFirstTime;

        void Start()
        {
            Debug.Log($"[GameStatusManager] Start() in client: enabled={enabled}, active={gameObject.activeInHierarchy}");
        }


        public override void OnNetworkSpawn()
        {
            Debug.Log($"[GameStatusManager] OnNetworkSpawn. IsServer: {IsServer}, IsClient: {IsClient}, IsHost: {IsHost}");
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
            Debug.Log($"[GameStatusManager] Update check - IsClient: {IsClient}, IsServer: {IsServer}, IsHost: {IsHost}");
            if (!IsHost) return;
            if (MatchInfo.Instance.GetAllConnected() && !allConectedFirstTime)
            {
                matchStateManager.SetMatchState(MatchState.starting);
                allConectedFirstTime = true;
            }
        }

        private void HandleStateChanged(MatchState state)
        {
            if(!IsServer) return;

            Debug.Log(state);
            if (state == MatchState.starting)
            {
                startingCoroutine = StartCoroutine(startMatchCountdown(5));
            }
            else if (state == MatchState.gameOver)
            {
                SetGameOverSceneRpc();
            }
            else if (state == MatchState.exit)
            {
                SetGameMenuSceneRpc();
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void SetGameOverSceneRpc()
        {
            SceneManager.LoadSceneAsync(Scenes.GameOverScene.ToString());
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void SetGameMenuSceneRpc()
        {
            SceneManager.LoadSceneAsync(Scenes.MenuScene.ToString());
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
            StopCoroutine(startingCoroutine);
            startingCoroutine = null;
        }
    }
}
