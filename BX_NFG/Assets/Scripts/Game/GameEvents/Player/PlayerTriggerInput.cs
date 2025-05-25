
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents.Player.Input
{
    public class PlayerTriggerInput : NetworkBehaviour
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
                ShootServerRpc(45);
            }
        }

        public void TryPass()
        {
            if (IsOwner)
            {
                ShootServerRpc(20);
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
