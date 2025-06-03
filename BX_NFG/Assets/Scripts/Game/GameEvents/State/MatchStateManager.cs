using Unity.Netcode;
using UnityEngine;
using System;
using Assets.Scripts.Commons;

namespace Assets.Scripts.GameManager.GameEvents.State
{
    public class MatchStateManager : NetworkSingleton<MatchStateManager>
    {
        public event Action<MatchState> OnMatchStateChanged;
        private MatchState currentState;

        private void Awake()
        {
            gameObject.AddComponent<NetworkObject>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Debug.Log($"OnNetworkSpawn {gameObject.name} ID:{gameObject.GetInstanceID()} || isServer: {IsServer} || isClient: {IsClient} || isHost: {IsHost} || isOwner: {IsOwner}");
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        [ClientRpc]
        private void NotifyMatchStateChangedClientRpc(MatchState newState)
        {
            Debug.Log($"[CLIENT] Match state changed to {newState}");
            OnMatchStateChanged?.Invoke(newState);
        }

        public void SetMatchState(MatchState newState)
        {
            Debug.Log($"SetMatchState {gameObject.name} ID:{gameObject.GetInstanceID()} => {newState} || isServer: {IsServer} || isClient: {IsClient} || isHost: {IsHost} || isOwner: {IsOwner}");
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
                //NotifyMatchStateChangedClientRpc(newState);
            }
        }
    }
}
