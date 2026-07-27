using Unity.Netcode;
using UnityEngine;
using System;
using Assets.Scripts.Commons;

namespace Assets.Scripts.GameManager.GameEvents.State
{
    public class MatchStateManager : NetworkBehaviour
    {
        public event Action<MatchState> OnMatchStateChanged;

        private MatchState currentState;

        public override void OnNetworkSpawn()
        {
        }

        public void SetMatchState(MatchState newState)
        {
            if (!IsSpawned)
            {
                return;
            }

            if (!IsServer)
            {
                return;
            }

            if (currentState != newState)
            {
                currentState = newState;

                OnMatchStateChanged?.Invoke(newState);

                NotifyMatchStateChangedClientRpc(newState);
            }
        }

        [ClientRpc]
        private void NotifyMatchStateChangedClientRpc(MatchState newState)
        {
            if (IsServer) return;

            currentState = newState;

            OnMatchStateChanged?.Invoke(newState);
        }

        public MatchState GetCurrentState() => currentState;
    }
}
