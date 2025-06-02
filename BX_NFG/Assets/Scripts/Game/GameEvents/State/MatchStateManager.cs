using Unity.Netcode;
using UnityEngine;
using System;
using Assets.Scripts.Commons;

namespace Assets.Scripts.GameManager.GameEvents.State
{
    public class MatchStateManager : NetworkSingleton<MatchStateManager>
    {
        public event Action<MatchState> OnMatchStateChanged;

        public NetworkVariable<MatchState> MatchState = new(default,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        private void Awake()
        {
            var netObj = GetComponent<NetworkObject>();
            if (netObj == null)
            {
                netObj = gameObject.AddComponent<NetworkObject>();
            }
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                MatchState.OnValueChanged += HandleMatchStateChanged;
            }
        }

        private void HandleMatchStateChanged(MatchState oldState, MatchState newState)
        {
            Debug.Log($"[CLIENT] Match state changed to {newState}");
            OnMatchStateChanged?.Invoke(newState);
        }

        // Solo el servidor puede cambiar el estado
        public void SetMatchState(MatchState newState)
        {
            Debug.Log($"AAAAAAAA => {newState} || isServer: {IsServer} || isClient: {IsClient} || isHost: {IsHost} || isOwner: {IsOwner}");

            if (!IsServer)
            {
                return;
            }
            if (MatchState.Value != newState)
            {
                MatchState.Value = newState;
                Debug.Log($"[SERVER] Match state changed to {newState}");
                OnMatchStateChanged?.Invoke(newState); 
            }
        }
    }
}
