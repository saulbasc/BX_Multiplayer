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

        public Task<bool> updates(Match entity)
        {
            throw new NotImplementedException();
        }
    }
}
