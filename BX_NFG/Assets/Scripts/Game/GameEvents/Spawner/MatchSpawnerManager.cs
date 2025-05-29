using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Assets.Scripts.GameManager.GameEvents
{
    public class MatchSpawnerManager : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                MatchStateManager.Instance.OnMatchStateChanged += HandleStateChanged;
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
            if(!IsServer) return;

            if (state == MatchState.starting || state == MatchState.preMatch)
            {
                foreach (var (playerId, playerInGame) in PlayerConnectionMap.Instance.PlayerInGameList)
                {
                    var networkTransform = playerInGame.GetComponent<NetworkTransform>();
                    if (networkTransform != null)
                    {
                        Debug.Log($"Teleporting player {playerId} to spawn position {playerInGame.spawnPosition}");
                        networkTransform.Teleport(playerInGame.spawnPosition, Quaternion.identity, new Vector3(1.5f, 1.5f, 1.5f));
                    }
                }
            }
        }
    }
}
