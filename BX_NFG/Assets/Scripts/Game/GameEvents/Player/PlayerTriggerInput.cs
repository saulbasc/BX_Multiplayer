using System.Collections;
using Assets.Scripts.Game.GameEvents.Ball;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents.Player.Input
{
    public class PlayerTriggerInput : NetworkBehaviour
    {
        private MatchStateManager matchStateManager;
        private NetworkVariable<bool> ballInRange = new NetworkVariable<bool>(false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);
        private NetworkVariable<bool> shootable = new NetworkVariable<bool>(false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                PlayerEvents.OnShootAction += TryShoot;
                PlayerEvents.OnPassAction += TryPass;
            }

            if (IsServer)
            {
                StartCoroutine(WaitForGameManager());
            }
        }

        private IEnumerator WaitForGameManager()
        {
            GameObject manager = null;

            while (manager == null)
            {
                manager = GameObject.Find("GameManager");
                yield return null;
            }

            matchStateManager = manager.GetComponent<MatchStateManager>();

            if (matchStateManager != null)
            {
                matchStateManager.OnMatchStateChanged += HandleStateChanged;
            }
            else
            {
                Debug.LogError("MatchStateManager component not found on GameManager!");
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsOwner)
            {
                PlayerEvents.OnShootAction -= TryShoot;
                PlayerEvents.OnPassAction -= TryPass;
            }

            if (IsServer)
            {
                if (matchStateManager != null)
                {
                    matchStateManager.OnMatchStateChanged -= HandleStateChanged;
                }
            }
        }

        private void HandleStateChanged(MatchState state)
        {
            if (state == MatchState.playing)
            {
                shootable.Value = true;
            }
            else
            {
                shootable.Value = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                if (IsServer)
                {
                    ballInRange.Value = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!IsServer) return;

            if (other.CompareTag("Ball"))
            {
                ballInRange.Value = false;
            }
        }

        public void TryShoot()
        {
            if (!IsOwner) return;
            ShootServerRpc(transform.position);
        }

        public void TryPass()
        {
            if (!IsOwner) return;
            PassServerRpc(transform.position);
        }

        [ServerRpc]
        private void ShootServerRpc(Vector3 playerPosition)
        {
            if (ballInRange.Value && shootable.Value)
            {
                BallManager.Instance.ShootBall(playerPosition);
            }
        }

        [ServerRpc]
        private void PassServerRpc(Vector3 playerPosition)
        {
            if (ballInRange.Value && shootable.Value)
            {
                BallManager.Instance.PassBall(playerPosition);
            }
        }
    }
}