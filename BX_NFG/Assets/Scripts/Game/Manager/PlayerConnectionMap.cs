using System.Collections.Generic;
using Assets.Scripts.Commons;
using Assets.Scripts.Game.GameEvents.Player;
using Unity.Netcode;
using UnityEngine;

public class PlayerConnectionMap : Singleton<PlayerConnectionMap>
{
    private Dictionary<ulong, PlayerInGame> playerInGameList = new();

    public Dictionary<ulong, PlayerInGame> PlayerInGameList
    {
        get { return playerInGameList; }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RegisterPlayer(ulong clientId, PlayerInGame playerInGame)
    {
        playerInGameList[clientId] = playerInGame;
        Debug.Log("Client registered in PlayerConnectionMap: " + clientId+ "with team: "+playerInGame.Team+" Name: "+playerInGame.TagName);
    }
}
