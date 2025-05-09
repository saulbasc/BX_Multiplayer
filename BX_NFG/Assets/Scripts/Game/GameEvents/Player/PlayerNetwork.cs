using Assets.Scripts.Game.Manager;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            NotifyConnectedServerRpc();
        }
    }

    [Rpc(SendTo.Server)]
    public void NotifyConnectedServerRpc()
    {
        MatchInfo.Instance.SetNumberOdPlayersInTeamsConnected(
            MatchInfo.Instance.NumberOfPlayersInTeamsConnected + 1
        );
    }
}
