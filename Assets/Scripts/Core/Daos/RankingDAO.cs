using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Assets.Scripts.Core.Models;
using Firebase.Firestore;
using UnityEngine;

namespace Assets.Scripts.Core.Daos
{
    public class RankingDAO : Singleton<RankingDAO>
    {
        private FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;

        private const string COLLECTION_NAME = "rankings";

        public async Task Insert(string playerId, RankingPlayerStats stats)
        {
            try
            {
                RankingPlayerStats currentStats = await Select(playerId);
                if (currentStats == null)
                {
                    Debug.LogWarning($"Player stats not found for {playerId}, initializing new.");
                    currentStats = new RankingPlayerStats();
                }

                currentStats.PlayerName = stats.PlayerName;
                currentStats.MatchesPlayed += stats.MatchesPlayed;
                currentStats.Goals += stats.Goals;
                currentStats.Touches += stats.Touches;
                currentStats.SecondsPlayed += stats.SecondsPlayed;

                DocumentReference docRef = firestore
                    .Collection(COLLECTION_NAME)
                    .Document(playerId);

                await docRef.SetAsync(currentStats, SetOptions.MergeAll);

                Debug.Log("INSERTADO CON EXITO");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error updating player stats: {e.Message}");
            }
        }

        public async Task<RankingPlayerStats> Select(string playerId)
        {
            try
            {
                DocumentReference docRef = firestore
                    .Collection(COLLECTION_NAME)
                    .Document(playerId);

                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                    return snapshot.ConvertTo<RankingPlayerStats>();

                return new RankingPlayerStats();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error getting player stats: {e.Message}");
                return null;
            }
        }

        public async Task<List<RankingStat>> GetRanking(RankingType type, int limit = 10)
        {
            string field = type.ToString();

            try
            {
                var snapshot = await firestore
                    .Collection(COLLECTION_NAME)
                    .OrderByDescending(field)
                    .Limit(limit)
                    .GetSnapshotAsync();

                List<RankingStat> result = new List<RankingStat>();

                foreach (var doc in snapshot.Documents)
                {
                    var stats = doc.ConvertTo<RankingPlayerStats>();

                    int value = 0;
                    switch (type)
                    {
                        case RankingType.Goals:
                            value = stats.Goals;
                            break;
                        case RankingType.MatchesPlayed:
                            value = stats.MatchesPlayed;
                            break;
                        case RankingType.Touches:
                            value = stats.Touches;
                            break;
                        case RankingType.SecondsPlayed:
                            value = (int)stats.SecondsPlayed / 60;
                            break;
                    }

                    result.Add(new RankingStat(stats.PlayerName, value));
                }

                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error al obtener ranking por {type}: {e.Message}");
                return new();
            }
        }

    }
}
