using System.Threading.Tasks;
using System;
using Assets.Scripts.Commons;
using Assets.Scripts.Core.Models;
using Firebase.Firestore;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Daos
{
    public class PlayerMatchSummaryDAO : Singleton<PlayerMatchSummaryDAO>
    {
        private FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;

        public async Task Insert(string playerId,  PlayerMatchSummary summary)
        {
            try
            {
                DocumentReference docRef = firestore
                    .Collection("usersData")
                    .Document(playerId)
                    .Collection("match_summaries")
                    .Document();

                await docRef.SetAsync(summary);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving match summary: {e.Message}");
            }
        }

        public async Task<PlayerMatchSummary> Select(string playerId, string matchId)
        {
            try
            {
                DocumentReference docRef = firestore
                    .Collection("usersData")
                    .Document(playerId)
                    .Collection("match_summaries")
                    .Document(matchId);

                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    return snapshot.ConvertTo<PlayerMatchSummary>();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error retrieving match summary: {e.Message}");
                return null;
            }
        }

        public async Task<List<PlayerMatchSummary>> SelectAll(string playerId)
        {
            try
            {
                CollectionReference collectionRef = firestore
                    .Collection("usersData")
                    .Document(playerId)
                    .Collection("match_summaries");
                QuerySnapshot snapshot = await collectionRef.GetSnapshotAsync();
                List<PlayerMatchSummary> summaries = new List<PlayerMatchSummary>();
                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    summaries.Add(doc.ConvertTo<PlayerMatchSummary>());
                }
                return summaries;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error retrieving all match summaries: {e.Message}");
                return new List<PlayerMatchSummary>();
            }
        }
    }
}
