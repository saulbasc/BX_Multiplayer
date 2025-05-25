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
        public PlayerTeam Team { get; private set; }
        public Vector3 spawnPosition { get; private set; }

        public override void OnNetworkSpawn()
        {
            SetPlayerConnected();
            PlayerConnectionID = OwnerClientId;
            LobbyPlayerData playerData = LobbyPlayerManager.Instance.GetSinglePlayerDataObject(UnityServicesActions.GetCurrentUserID());
            PlayerId = playerData.Id;
            Team = playerData.PlayerTeam;

            Vector3 spawn = SpawnPositions.GetNextSpawn(Team);
            spawnPosition = spawn;
        }

        public void SetPlayerConnected()
        {
            MatchInfo.Instance.SetNumberOdPlayersInTeamsConnected(
                MatchInfo.Instance.NumberOfPlayersInTeamsConnected + 1
            );
        }
    }
}
