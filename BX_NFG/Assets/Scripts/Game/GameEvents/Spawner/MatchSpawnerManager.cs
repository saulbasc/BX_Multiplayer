
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
            if(state == MatchState.starting)
            {
                
            }
        }
    }
}
