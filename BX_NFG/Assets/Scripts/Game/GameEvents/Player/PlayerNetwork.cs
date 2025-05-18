using Assets.Scripts.Game.Manager;
using Unity.Netcode;
using UnityEngine;

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
        Debug.Log("Recibido aviso de nueva conexión");
        MatchInfo.Instance.SetNumberOdPlayersInTeamsConnected(
            MatchInfo.Instance.NumberOfPlayersInTeamsConnected + 1
        );
    }
}
