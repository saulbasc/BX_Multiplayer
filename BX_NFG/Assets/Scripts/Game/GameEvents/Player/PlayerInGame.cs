
using Assets.Scripts.Game.GameEvents.Spawner;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Logic;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents.Player
{
    public class PlayerInGame : NetworkBehaviour
    {
        public string PlayerId { get; private set; }
        public PlayerTeam Team { get; private set; }

        private NetworkVariable<Vector3> spawnPosition = new(new Vector3() ,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                string id = PlayerConnectionMap.Instance.GetByClientId(OwnerClientId)?.Id;
                PlayerId = id;
                PlayerTeam team = LobbyPlayersManager.Instance.GetPlayerTeam(PlayerId);
                Team = team;

                Vector3 spawn = SpawnPositions.GetNextSpawn(Team);
                spawnPosition.Value = spawn;
                transform.position = spawn;
            }
            else
            {
                spawnPosition.OnValueChanged += (oldPos, newPos) =>
                {
                    transform.position = newPos;
                };
            }
        }
    }
}
