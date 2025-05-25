using System.Collections.Generic;
using Assets.Scripts.Lobbi.Data;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents.Spawner
{
    public static class SpawnPositions
    {
        private static List<Vector3> localTeamSpawns = new()
        {
            new Vector3(-30, 1, -15),
            new Vector3(-30, 1, -7),
            new Vector3(-30, 1,  0),
            new Vector3(-30, 1,  7),
        };

        private static List<Vector3> visitorTeamSpawns = new()
        {
            new Vector3(30, 1, -15),
            new Vector3(30, 1, -7),
            new Vector3(30, 1, 0),
            new Vector3(30, 1, 7),
        };

        private static Dictionary<PlayerTeam, int> spawnIndices = new()
        {
            { PlayerTeam.Local, 0 },
            { PlayerTeam.Visitor, 0 }
        };

        public static Vector3 GetNextSpawn(PlayerTeam team)
        {
            Debug.Log("Player TEAMMMMMMMMMMMMMM => " + team);
            var list = team == PlayerTeam.Local ? localTeamSpawns : visitorTeamSpawns;
            int index = spawnIndices[team];

            if (index >= list.Count)
            {
                Debug.LogWarning($"No hay más spawns para el equipo {team}. Usando la última.");
                return list[^1]; 
            }

            Vector3 spawn = list[index];
            spawnIndices[team]++;
            Debug.Log($"Spawn para el equipo {team}: {spawn} (Índice: {index})");
            return spawn;
        }

        public static void Reset()
        {
            spawnIndices[PlayerTeam.Local] = 0;
            spawnIndices[PlayerTeam.Visitor] = 0;
        }
    }
}
