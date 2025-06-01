using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

namespace Assets.Scripts.Core.Models
{
    [FirestoreData]
    public class Team
    {
        [FirestoreProperty]
        public int Score { get; set; }

        [FirestoreProperty]
        public Dictionary<string, GamePlayer> Players { get; set; }

        public Team(int score, Dictionary<string, GamePlayer> players)
        {
            Score = score;
            Players = players;
        }

        public Team()
        {
            Score = 0;
            Players = new Dictionary<string, GamePlayer>();
        }

        public void AddNewPlayer(ulong playerGameId,string id)
        {
            Debug.Log("Adding new player to team: " + id+ " Y GAME ID => "+playerGameId);
            Players[id] = new GamePlayer(playerGameId, id);
        }

        public void AddGoal()
        {
            Score++;
        }
    }
}
