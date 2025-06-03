using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Handlers;
using Firebase.Firestore;
using UnityEngine;

namespace Assets.Scripts.Core.Daos
{
    public class MatchDAO : Singleton<MatchDAO>, IDAO<Match, string>
    {
        private FirebaseFirestore firestore = FirebaseFirestore.DefaultInstance;
        private const string COLLECTION_NAME = "match";

        public Task<bool> delete(string id)
        {
            DocumentReference docRef = firestore.Collection(COLLECTION_NAME).Document(id);
            return SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                await docRef.DeleteAsync();
                return true;
            }, false); 
        }

        public Task<bool> insert(Match entity)
        {
            CollectionReference collectionRef = firestore.Collection(COLLECTION_NAME);
            return SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                await collectionRef.AddAsync(entity);
                Debug.Log("Match Insertado");
                return true;
            }, false);
        }

        public Task<Match> select(string id)
        {
            DocumentReference docRef = firestore.Collection(COLLECTION_NAME).Document(id);
            return SafeAsyncFunctionsHandler.ExecuteAsync<Match>(async () =>
            {
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    Match match = snapshot.ConvertTo<Match>();
                    return match;
                }
                return null;
            }, null);
        }

        public Task<List<Match>> selectAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Match>> SelectAllWithPlayerID(string firebaseID)
        {
            CollectionReference collectionRef = firestore.Collection(COLLECTION_NAME);

            return SafeAsyncFunctionsHandler.ExecuteAsync<List<Match>>(async () =>
            {
                QuerySnapshot snapshot = await collectionRef.GetSnapshotAsync();
                List<Match> matches = new List<Match>();

                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    Match match = document.ConvertTo<Match>();

                    bool playerFound = false;

                    if (match.LocalTeam != null && match.LocalTeam.Players != null)
                    {
                        if (match.LocalTeam.Players.ContainsKey(firebaseID))
                        {
                            playerFound = true;
                        }
                    }

                    if (!playerFound && match.VisitorTeam != null && match.VisitorTeam.Players != null)
                    {
                        if (match.VisitorTeam.Players.ContainsKey(firebaseID))
                        {
                            playerFound = true;
                        }
                    }

                    if (playerFound)
                    {
                        matches.Add(match);
                    }
                }

                return matches;
            }, new List<Match>());
        }

        public Task<bool> updates(Match entity)
        {
            throw new NotImplementedException();
        }
    }
}
