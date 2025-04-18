using UnityEngine;
using System;
using Unity.Netcode;

namespace Assets.Scripts.GameManager.GameEvents.State
{
    public class MatchStateManager : NetworkBehaviour
    {
        public event Action<MatchState> OnMatchStateChanged;
        [SerializeField] private MatchState matchState;

        public void SetMatchState(MatchState matchState)
        {
            if(this.matchState != matchState)
            {
                this.matchState = matchState;
                OnMatchStateChanged?.Invoke(matchState);
                Debug.Log(matchState);
            }
        }
    }
}
