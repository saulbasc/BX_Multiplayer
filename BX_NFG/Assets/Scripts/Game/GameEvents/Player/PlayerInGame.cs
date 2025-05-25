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

        private NetworkVariable<Vector3> spawnPosition = new(new Vector3() ,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        public override void OnNetworkSpawn()
        {
            SetPlayerConnected();
            PlayerConnectionID = OwnerClientId;
            LobbyPlayerData playerData = LobbyPlayerManager.Instance.GetSinglePlayerDataObject(UnityServicesActions.GetCurrentUserID());
            PlayerId = playerData.Id;
            Team = playerData.PlayerTeam;

            Vector3 spawn = SpawnPositions.GetNextSpawn(Team);

            if (IsServer)
            {
                spawnPosition.Value = spawn;
                transform.position = spawn;
            }

            Debug.Log(spawnPosition.Value + " " + PlayerId + " " + PlayerConnectionID + " " + Team);  
        }

        public void SetPlayerConnected()
        {
            MatchInfo.Instance.SetNumberOdPlayersInTeamsConnected(
                MatchInfo.Instance.NumberOfPlayersInTeamsConnected + 1
            );
        }
    }
}
