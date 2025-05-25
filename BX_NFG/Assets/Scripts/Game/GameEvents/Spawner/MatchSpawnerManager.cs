using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.GameManager.GameEvents
{
    public class MatchSpawnerManager : NetworkBehaviour
    {
        [SerializeField] private MatchStateManager matchStateManager;

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
            Debug.Log("Ahora mismo hay " + PlayerConnectionMap.Instance.PlayerInGameList.Count + " jugadores en el mapa.");
            if ( state == MatchState.starting)
            {
                foreach (var (playerId, playerInGame) in PlayerConnectionMap.Instance.PlayerInGameList)
                {
                    Debug.Log($"Spawning player {playerId} at their spawn position: {playerInGame.spawnPosition}");
                    playerInGame.gameObject.transform.position = playerInGame.spawnPosition;
                }
            }
        }
    }
}
