
using System.Collections;
using Assets.Scripts.Commons;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents.Ball
{
    public class BallManager : NetworkSingleton<BallManager>
    {
        [SerializeField] private GameObject ballPrefab;
        private GameObject ball;
        private BallController ballController;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                MatchStateManager.Instance.OnMatchStateChanged += HandleStateChanged;
                ball = Instantiate(ballPrefab, new Vector3(0, 2.5f, 0), Quaternion.identity);
                ballController = ball.GetComponent<BallController>();
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                MatchStateManager.Instance.OnMatchStateChanged -= HandleStateChanged;
            }
        }

        private void HandleStateChanged(MatchState state)
        {
            if (state == MatchState.starting)
            {
                InitialBallSpawn();
            }
            else if (state == MatchState.pause)
            {
                ballController.PauseBall();
            }
            else if (state == MatchState.playing)
            {
                ballController.ResumeBall();
            }
        }

        private void InitialBallSpawn()
        {
            if (ballPrefab != null)
            {
                var networkBall = ball.GetComponent<NetworkObject>();
                if (!networkBall.IsSpawned)
                {
                    ball.GetComponent<NetworkObject>().Spawn();
                }
                StartCoroutine(ApplyInitialVelocity(ball));
            }
        }

        private IEnumerator ApplyInitialVelocity(GameObject ball)
        {
            yield return new WaitForFixedUpdate();
            ball.transform.position = new Vector3(0, 1.75f, 0);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            Vector3 ballForce = new Vector3(0, 0, 1);
        }
    }
}