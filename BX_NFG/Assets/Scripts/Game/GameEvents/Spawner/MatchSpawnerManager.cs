using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;

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
            if ( state == MatchState.starting)
            {
                foreach (var (playerId, playerInGame) in PlayerConnectionMap.Instance.PlayerInGameList)
                {
                    playerInGame.gameObject.transform.position = playerInGame.spawnPosition;
                }
            }
        }
    }
}
