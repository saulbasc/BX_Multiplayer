
using System.Collections;
using Assets.Scripts.Commons;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents.Ball
{
    public class BallManager : NetworkSingleton<BallManager>
    {
        [SerializeField] private MatchStateManager matchStateManager;
        [SerializeField] private GameObject ballPrefab;
        private GameObject ball;
        private BallController ballController;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                matchStateManager.OnMatchStateChanged += HandleStateChanged;
                ball = Instantiate(ballPrefab, new Vector3(0, 1.75f, 0), Quaternion.identity);
                ballController = ball.GetComponent<BallController>();
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                matchStateManager.OnMatchStateChanged -= HandleStateChanged;
            }
        }

        private void HandleStateChanged(MatchState state)
        {
            if (state == MatchState.starting)
            {
                InitialBallSpawn();
            }
            else if(state == MatchState.pause)
            {
                Debug.Log("Bola pausada?");
                ballController.PauseBall();
            }
            else if (state == MatchState.playing)
            {
                Debug.Log("Bola activa?");
                ballController.ResumeBall();
            }
        }

        private void InitialBallSpawn()
        {
            if (ballPrefab != null)
            {
                ball.GetComponent<NetworkObject>().Spawn();
                StartCoroutine(ApplyInitialVelocity(ball));
            }
        }

        private IEnumerator ApplyInitialVelocity(GameObject ball)
        {
            yield return new WaitForFixedUpdate();

            Rigidbody rb = ball.GetComponent<Rigidbody>();
            Vector3 ballForce = new Vector3(0, 0, 1);
        }

    }
}
