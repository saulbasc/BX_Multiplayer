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
                ball = Instantiate(ballPrefab, new Vector3(0, 2.5f, 0), Quaternion.identity);
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
            if (ballPrefab != null && IsServer)
            {
                var networkBall = ball.GetComponent<NetworkObject>();
                ball.transform.position = new Vector3(0, 1.75f, 0);
                if (!networkBall.IsSpawned)
                {
                    ball.GetComponent<NetworkObject>().Spawn();
                }
            }
        }

        public void ShootBall(Vector3 playerPosition)
        {
            if(!IsServer || ballController == null) return;

            ballController.ShootBall(8f, playerPosition, OwnerClientId);
        }

        public void PassBall(Vector3 playerPosition)
        {
            if (!IsServer || ballController == null) return;

            ballController.ShootBall(3f, playerPosition, OwnerClientId);
        }
    }
}