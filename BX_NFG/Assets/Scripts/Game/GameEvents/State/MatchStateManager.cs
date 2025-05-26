using UnityEngine;
using System;
using Assets.Scripts.Commons;

namespace Assets.Scripts.GameManager.GameEvents.State
{
    public class MatchStateManager : NetworkSingleton<MatchStateManager>
    {
        public event Action<MatchState> OnMatchStateChanged;
        private MatchState matchState;

        public void SetMatchState(MatchState matchState)
        {
            if (this.matchState != matchState)
            {
                this.matchState = matchState;
                OnMatchStateChanged?.Invoke(matchState);
                Debug.Log(matchState);
            }
        }
    }
}
