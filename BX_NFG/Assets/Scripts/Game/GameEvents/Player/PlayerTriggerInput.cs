using Assets.Scripts.Game.GameEvents.Ball;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.Init;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents.Player.Input
{
    public class PlayerTriggerInput : NetworkBehaviour
    {
        private bool ballInRange;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                PlayerEvents.OnShootAction += TryShoot;
                PlayerEvents.OnPassAction += TryPass;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsOwner)
            {
                PlayerEvents.OnShootAction -= TryShoot;
                PlayerEvents.OnPassAction -= TryPass;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                if (IsServer)
                {
                    ballInRange = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(!IsServer) return;

            if (other.CompareTag("Ball"))
            {
                ballInRange = false;
            }
        }

        public void TryShoot()
        {
            Debug.Log("ENTRA EN SHOOT");
            if (!IsOwner) return;

            if (ballInRange)
            {
                Debug.Log("ENTRA EN SHOOT Y ESTA EN RANGO");
                ShootServerRpc(transform.position);
            }
        }

        public void TryPass()
        {
            if (!IsOwner) return;

            if (ballInRange)
            {
                PassServerRpc(transform.position);
            }
        }

        [ServerRpc]
        private void ShootServerRpc(Vector3 playerPosition)
        {
            BallManager.Instance.ShootBall(playerPosition);
        }

        [ServerRpc]
        private void PassServerRpc(Vector3 playerPosition)
        {
            BallManager.Instance.PassBall(playerPosition);
        }
    }
}