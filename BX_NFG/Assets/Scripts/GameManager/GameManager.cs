using Assets.Scripts.GameManager.GameEvents.State;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    private MatchStateManager matchStateManager;
    private MatchTimerManager matchTimerManager;

    private void Start()
    {
        if (!IsServer) return;

        matchStateManager.OnMatchStateChanged += OnMatchStateChanged;
        matchTimerManager.Init(MatchDuration.matchDuration1);
    }

    private void OnMatchStateChanged( MatchState matchState )
    {
        if( IsServer)
        {
            
        }
    }
}
