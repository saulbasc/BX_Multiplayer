
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents
{
    public class ColisionsManager : NetworkBehaviour
    {
        [SerializeField] private MatchStateManager matchStateManager;
        [SerializeField] private GameObject restartColision;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                matchStateManager.OnMatchStateChanged += HandleStateChanged;
            }
        }

        public override void OnNetworkDespawn()
        {
            matchStateManager.OnMatchStateChanged -= HandleStateChanged;
        }

        private void HandleStateChanged(MatchState state)
        {
            if(state == MatchState.starting)
            {
                restartColision.gameObject.SetActive(true);
            }
            else
            {
                restartColision.gameObject.SetActive(false);
            }
        }
    }
}
