using System;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Game.GameEvents.Spawner;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Lobbi.Players;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents.Player
{
    public class PlayerInGame : NetworkBehaviour
    {
        public ulong PlayerConnectionID { get; private set; }
        public string PlayerId { get; private set; }
        public string TagName { get; private set; }
        public PlayerTeam Team { get; private set; }
        public Vector3 spawnPosition { get; private set; }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                string userId = UnityServicesActions.GetCurrentUserID();
                SendDataServerRpc(userId);
            }
        }

        public static event Action<PlayerInGame> OnPlayerDataInitialized;

        [ServerRpc(RequireOwnership = true)]
        public void SendDataServerRpc(string userId, ServerRpcParams rpcParams = default)
        {
            LobbyPlayerData playerData = LobbyPlayerManager.Instance.GetSinglePlayerDataObject(userId);
            if(playerData.PlayerTeam == PlayerTeam.Spectator)
            {
                Debug.LogWarning("Player is a spectator and cannot join the game.");
                return;
            }

            SetPlayerConnected();
            PlayerConnectionID = rpcParams.Receive.SenderClientId;
            PlayerId = userId;

            TagName = playerData.GameTag;
            Team = playerData.PlayerTeam;

            spawnPosition = SpawnPositions.GetNextSpawn(Team);

            Debug.Log("Lobby Player DATA Id => " + playerData.Id + ", Tag => " + playerData.GameTag + ", Team => " + playerData.PlayerTeam);

            GamePlayerInfo gamePlayerInfo = GetComponent<GamePlayerInfo>();
        }

        public void SetPlayerConnected()
        {
            MatchInfo.Instance.SetNumberOdPlayersInTeamsConnected(
                MatchInfo.Instance.NumberOfPlayersInTeamsConnected + 1
            );
        }
    }
}
