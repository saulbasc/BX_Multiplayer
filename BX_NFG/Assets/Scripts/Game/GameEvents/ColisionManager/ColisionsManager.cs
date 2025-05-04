
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents
{
    public class ColisionsManager : NetworkBehaviour
    {
        [SerializeField] private GameObject restartColision;
        [SerializeField] private MatchStateManager matchStateManager;

        public override void OnNetworkSpawn()
        {
            matchStateManager.OnMatchStateChanged += HandleStateChanged;
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
