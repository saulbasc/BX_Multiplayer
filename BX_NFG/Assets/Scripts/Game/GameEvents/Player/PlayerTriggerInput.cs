using System;
using Assets.Scripts.Game.GameEvents.Ball;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents.Player.Input
{
    public class PlayerTriggerInput : NetworkBehaviour
    {
        private bool ballInRange;
        private bool shootable;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                PlayerEvents.OnShootAction += TryShoot;
                PlayerEvents.OnPassAction += TryPass;
            }

            if (IsServer)
            {
                MatchStateManager.Instance.OnMatchStateChanged += HandleStateChanged;
            }
        }

        private void HandleStateChanged(MatchState state)
        {
            if (state == MatchState.playing)
            {
                shootable = true;
            }
            else
            {
                shootable = false;
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
            Debug.Log("ENTRA EN SHOOT Y ESTA EN RANGO");
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
            if (ballInRange && shootable)
            {
                BallManager.Instance.ShootBall(playerPosition);
            }
        }

        [ServerRpc]
        private void PassServerRpc(Vector3 playerPosition)
        {
            if (ballInRange && shootable)
            {
                BallManager.Instance.PassBall(playerPosition);
            }
        }
    }
}