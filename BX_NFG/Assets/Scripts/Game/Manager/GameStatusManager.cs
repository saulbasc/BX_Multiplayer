using System.Collections;
using Assets.Scripts.Commons;
using Assets.Scripts.Core.Daos;
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
            if (!IsHost) return;
            if (MatchInfo.Instance.GetAllConnected() && !allConectedFirstTime)
            {
                MatchStateManager.Instance.SetMatchState(MatchState.starting);
                allConectedFirstTime = true;
            }
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
                MatchDAO.Instance.insert(MatchInfo.Instance.Match);
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

        private IEnumerator StartMatchCountdown(int countdownTime)
        {
            while (countdownTime > 0)
            {
                Debug.Log("Comenzando en " + countdownTime);
                yield return new WaitForSeconds(1);
                countdownTime--;
            }

            MatchStateManager.Instance.SetMatchState(MatchState.playing);
        }
    }
}
