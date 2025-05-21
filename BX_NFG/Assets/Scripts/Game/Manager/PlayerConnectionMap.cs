using System.Collections.Generic;
using Assets.Scripts.Commons;
using Assets.Scripts.Lobbi.Players;
using Unity.Netcode;
using UnityEngine;

public class PlayerConnectionMap : Singleton<PlayerConnectionMap>
{
    private Dictionary<ulong, LobbyPlayerData> clientIdToLobbyData = new();
    private Dictionary<string, ulong> playerIdToClientId = new();

    [ServerRpc(RequireOwnership = false)]
    public void RegisterForClientsRpc(ulong clientId, LobbyPlayerData lobbyData)
    {
        clientIdToLobbyData[clientId] = lobbyData;
        playerIdToClientId[lobbyData.Id] = clientId;
    }

    public void RegisterForHost(ulong clientId, LobbyPlayerData lobbyData)
    {
        clientIdToLobbyData[clientId] = lobbyData;
        playerIdToClientId[lobbyData.Id] = clientId;
        Debug.Log($"Registering host {clientId} with player ID {lobbyData.Id}");
    }

    public LobbyPlayerData GetByClientId(ulong clientId)
    {
        return clientIdToLobbyData.TryGetValue(clientId, out var data) ? data : null;
    }

    public ulong? GetClientIdByPlayerId(string playerId)
    {
        return playerIdToClientId.TryGetValue(playerId, out var clientId) ? clientId : null;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UnregisterClientsRpc(ulong clientId)
    {
        if (clientIdToLobbyData.TryGetValue(clientId, out var data))
        {
            playerIdToClientId.Remove(data.Id);
            clientIdToLobbyData.Remove(clientId);
        }
    }

    public void UnregisterHost(ulong clientId)
    {
        if (clientIdToLobbyData.TryGetValue(clientId, out var data))
        {
            playerIdToClientId.Remove(data.Id);
            clientIdToLobbyData.Remove(clientId);
        }
    }

    public void Clear()
    {
        clientIdToLobbyData.Clear();
        playerIdToClientId.Clear();
    }
}
