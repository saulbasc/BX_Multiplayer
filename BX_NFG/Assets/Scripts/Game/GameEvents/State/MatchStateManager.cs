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
            Debug.Log($"OnNetworkSpawn {gameObject.name} ID:{gameObject.GetInstanceID()} || isServer: {IsServer} || isClient: {IsClient} || isHost: {IsHost} || isOwner: {IsOwner}");
        }

        public void SetMatchState(MatchState newState)
        {
            if (!IsSpawned)
            {
                Debug.LogWarning("SetMatchState called before NetworkSpawn!");
                return;
            }

            if (!IsServer)
            {
                return;
            }

            if (currentState != newState)
            {
                currentState = newState;
                Debug.Log($"[SERVER] Match state changed to {newState}");

                OnMatchStateChanged?.Invoke(newState);

                NotifyMatchStateChangedClientRpc(newState);
            }
        }

        [ClientRpc]
        private void NotifyMatchStateChangedClientRpc(MatchState newState)
        {
            if (IsServer) return;

            currentState = newState;
            Debug.Log($"[CLIENT] Match state changed to {newState}");

            OnMatchStateChanged?.Invoke(newState);
        }

        public MatchState GetCurrentState() => currentState;
    }
}
