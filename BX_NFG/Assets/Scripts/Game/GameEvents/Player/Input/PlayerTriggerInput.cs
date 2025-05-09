
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents.Player.Input
{
    public class PlayerTriggerInput : PlayerNetwork
    {
        private GameObject ballInRange;

        public override void OnNetworkSpawn()
        {
            PlayerEvents.OnShootAction += TryShoot;
            PlayerEvents.OnPassAction += TryPass;
        }

        public override void OnNetworkDespawn()
        {
            PlayerEvents.OnShootAction -= TryShoot;
            PlayerEvents.OnPassAction -= TryPass;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                ballInRange = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ball") && ballInRange == other.gameObject)
            {
                ballInRange = null;
            }
        }

        public void TryShoot()
        {
            if (IsOwner)
            {
                ShootServerRpc(30);
            }
        }

        public void TryPass()
        {
            if (IsOwner)
            {
                ShootServerRpc(15);
            }
        }

        [Rpc(SendTo.Server)]
        private void ShootServerRpc(float shootForce)
        {
            if (ballInRange != null)
            {
                Rigidbody ballRb = ballInRange.GetComponent<Rigidbody>();

                Vector3 direction = (ballRb.position - transform.position).normalized;

                ballRb.AddForce(direction * shootForce, ForceMode.Impulse);
            }
        }

    }
}
