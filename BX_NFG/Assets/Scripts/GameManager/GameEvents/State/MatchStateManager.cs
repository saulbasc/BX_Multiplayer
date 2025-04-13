using UnityEngine;
using System;

namespace Assets.Scripts.GameManager.GameEvents.State
{
    public class MatchStateManager : MonoBehaviour
    {
        public event Action<MatchState> OnMatchStateChanged;
        private MatchState matchState;

        public void SetMatchState(MatchState matchState)
        {
            if(this.matchState != matchState)
            {
                this.matchState = matchState;
                OnMatchStateChanged?.Invoke(matchState);
            }
        }
    }
}
