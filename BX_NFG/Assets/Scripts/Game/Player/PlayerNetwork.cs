using Assets.Scripts.Game.Manager;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            Debug.Log("EEEEEEEEEEEEEEEEEEE");
            NotifyConnectedServerRpc();
        }
    }

    [Rpc(SendTo.Server)]
    public void NotifyConnectedServerRpc()
    {
        Debug.Log("OOOOOOOOOOOOOOOO");
        MatchInfo.Instance.SetNumberOdPlayersInTeamsConnected(
            MatchInfo.Instance.NumberOfPlayersInTeamsConnected + 1
        );
        Debug.Log("Número de jugadores conectados actualizado a: " + MatchInfo.Instance.NumberOfPlayersInTeamsConnected);
    }
}
