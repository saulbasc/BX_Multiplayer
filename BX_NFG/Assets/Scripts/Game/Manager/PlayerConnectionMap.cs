using System.Collections.Generic;
using Assets.Scripts.Commons;
using Assets.Scripts.Lobbi.Players;

public class PlayerConnectionMap : Singleton<PlayerConnectionMap>
{
    private Dictionary<ulong, LobbyPlayerData> clientIdToLobbyData = new();
    private Dictionary<string, ulong> playerIdToClientId = new();

    public void Register(ulong clientId, LobbyPlayerData lobbyData)
    {
        clientIdToLobbyData[clientId] = lobbyData;
        playerIdToClientId[lobbyData.Id] = clientId;
    }

    public LobbyPlayerData GetByClientId(ulong clientId)
    {
        return clientIdToLobbyData.TryGetValue(clientId, out var data) ? data : null;
    }

    public ulong? GetClientIdByPlayerId(string playerId)
    {
        return playerIdToClientId.TryGetValue(playerId, out var clientId) ? clientId : null;
    }

    public void Unregister(ulong clientId)
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
