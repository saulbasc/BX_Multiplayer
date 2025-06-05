using System;
using System.Collections;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Assets.Scripts.GameManager.GameEvents
{
    public class MatchSpawnerManager : NetworkBehaviour
    {
        [SerializeField] private MatchStateManager matchStateManager;
        public static event Action<bool> OnTeleportingChanged;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                matchStateManager.OnMatchStateChanged += HandleStateChanged;
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
            if (!IsServer) return;

            if (state == MatchState.starting || state == MatchState.preMatch)
            {
                StartCoroutine(TeleportPlayers());
            }
        }

        private IEnumerator TeleportPlayers()
        {
            OnTeleportingChanged?.Invoke(true);

            yield return new WaitForSeconds(0.05f); 

            foreach (var (playerId, playerInGame) in PlayerConnectionMap.Instance.PlayerInGameList)
            {
                var rb = playerInGame.GetComponent<Rigidbody>();
                var networkTransform = playerInGame.GetComponent<NetworkTransform>();

                if (networkTransform != null) networkTransform.enabled = false;

                if (rb != null && playerInGame.PlayerId != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.position = playerInGame.spawnPosition;
                }

                if (networkTransform != null)
                {
                    StartCoroutine(ReenableTransform(networkTransform));
                }
            }

            yield return new WaitForSeconds(0.1f); 
            OnTeleportingChanged?.Invoke(false);
            Debug.Log("✅ Teleport finalizado");
        }

        private IEnumerator ReenableTransform(NetworkTransform netTransform)
        {
            yield return null;
            netTransform.enabled = true;
        }
    }
}
