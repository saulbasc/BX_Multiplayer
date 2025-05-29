using Assets.Scripts.Game.Manager;
using Assets.Scripts.Init;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents.Player.Input
{
    public class PlayerTriggerInput : NetworkBehaviour
    {
        private GameObject ballInRange;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                PlayerEvents.OnShootAction += TryShoot;
                PlayerEvents.OnPassAction += TryPass;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collision detected with: " + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Ball"))
            {
                RegisterTouchServerRpc(UnityServicesActions.GetCurrentUserID());
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void RegisterTouchServerRpc(string id)
        {
            Debug.Log("Touch registrado para el jugador con ID: " + id);
            foreach (var (key, player) in MatchInfo.Instance.Match.LocalTeam.Players)
            {
                if (key.Equals(id))
                {
                    player.AddTouch();
                    return;
                }
            }

            foreach (var (key, player) in MatchInfo.Instance.Match.VisitorTeam.Players)
            {
                if (key.Equals(id))
                {
                    player.AddTouch();
                    return;
                }
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
                ballInRange = other.gameObject;
                RegisterTouchServerRpc(UnityServicesActions.GetCurrentUserID());
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
            if (!IsOwner || !ballInRange) return;

            Rigidbody ballRb = ballInRange.GetComponent<Rigidbody>();
            Vector3 direction = (ballRb.position - transform.position).normalized;
            float shootForce = 6f;

            ballRb.AddForce(direction * shootForce, ForceMode.Impulse);
            ShootServerRpc(shootForce);
        }

        public void TryPass()
        {
            if (!IsOwner || !ballInRange) return;

            Rigidbody ballRb = ballInRange.GetComponent<Rigidbody>();
            Vector3 direction = (ballRb.position - transform.position).normalized;
            float shootForce = 2f;

            ballRb.AddForce(direction * shootForce, ForceMode.Impulse);
            ShootServerRpc(shootForce);
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