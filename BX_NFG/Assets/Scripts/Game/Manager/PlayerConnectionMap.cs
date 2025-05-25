using System.Collections.Generic;
using Assets.Scripts.Commons;
using Assets.Scripts.Game.GameEvents.Player;
using UnityEngine;

public class PlayerConnectionMap : Singleton<PlayerConnectionMap>
{
    private readonly Dictionary<ulong, PlayerInGame> playerInGameList = new();

    public void RegisterPlayer(ulong clientId, PlayerInGame playerInGame)
    {
        playerInGameList[clientId] = playerInGame;
        Debug.Log("Client registered in PlayerConnectionMap: " + clientId);
    }

    public void MovePlayer(ulong clientId)
    {
        if(playerInGameList.TryGetValue(clientId, out var playerNetwork))
        {
            playerNetwork.transform.position = Vector3.zero;
            Debug.Log($"Moved player {clientId} to the origin.");
        }
        else
        {
            Debug.LogWarning($"No player controller found for client {clientId}.");
        }
    }
}
