using System.Threading.Tasks;
using System;
using Assets.Scripts.Commons;
using Assets.Scripts.Core.Models;
using Firebase.Firestore;
using UnityEngine;

namespace Assets.Scripts.Core.Daos
{
    public class PlayerStatsDAO : Singleton<PlayerStatsDAO>
    {
        private FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;

        public async Task<PlayerStats> Select(string playerId)
        {
            try
            {
                DocumentReference docRef = firestore
                    .Collection("users")
                    .Document(playerId)
                    .Collection("data")
                    .Document("player_stats");

                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                    return snapshot.ConvertTo<PlayerStats>();

                return new PlayerStats(); 
            }
            catch (Exception e)
            {
                Debug.LogError($"Error getting player stats: {e.Message}");
                return null;
            }
        }

        public async Task Insert(string playerId, PlayerStats deltaStats)
        {
            try
            {
                PlayerStats currentStats = await Select(playerId);
                if (currentStats == null)
                {
                    Debug.LogWarning($"Player stats not found for {playerId}, initializing new.");
                    currentStats = new PlayerStats();
                }

                currentStats.MatchesPlayed += deltaStats.MatchesPlayed;
                currentStats.Goals += deltaStats.Goals;
                currentStats.Touches += deltaStats.Touches;
                currentStats.MinutesPlayed += deltaStats.MinutesPlayed;

                DocumentReference docRef = firestore
                    .Collection("usersData")
                    .Document(playerId)
                    .Collection("data")
                    .Document("player_stats");

                await docRef.SetAsync(currentStats, SetOptions.MergeAll);

                Debug.Log("INSERTADO CON EXITO");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error updating player stats: {e.Message}");
            }
        }
    }
}
