using System;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Game.GameEvents.Spawner;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Lobbi.Players;
using Assets.Scripts.UI.MenuUI.Components;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents.Player
{
    public class PlayerInGame : NetworkBehaviour
    {
        public ulong PlayerConnectionID { get; private set; }
        public string PlayerId { get; private set; }
        public string TagName { get; private set; }
        public PlayerTeam Team { get; private set; }
        public Vector3 spawnPosition { get; private set; }

        private int goals;
        private int touches;
        private float secondsPlayed;

        public int Goals => goals;
        public int Touches => touches;
        public float SecondsPlayed => secondsPlayed;

        public void AddGoal()
        {
            Debug.Log("GOAL AÑADIDO A PLAYER IN GAME");
            goals++;
        }
        public void RegisterTouch() => touches++;
        public void AddTime(float deltaTime) => secondsPlayed += deltaTime;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                string userId = UnityServicesActions.GetCurrentUserID();
                SendDataServerRpc(userId);
            }
        }

        private void Update()
        {
            if (!IsServer) return;

            AddTime(Time.deltaTime);
        }

        public static event Action<PlayerInGame> OnPlayerDataInitialized;

        [ServerRpc(RequireOwnership = true)]
        public void SendDataServerRpc(string userId, ServerRpcParams rpcParams = default)
        {
            LobbyPlayerData playerData = LobbyPlayerManager.Instance.GetSinglePlayerDataObject(userId);
            if(playerData.PlayerTeam == PlayerTeam.Spectator)  return;

            SetPlayerConnected();
            PlayerConnectionID = rpcParams.Receive.SenderClientId;
            PlayerId = userId;

            TagName = playerData.GameTag;
            Team = playerData.PlayerTeam;

            spawnPosition = SpawnPositions.GetNextSpawn(Team);

            goals = 0;
            touches = 0;
            secondsPlayed = 0;

            Debug.Log("Lobby Player DATA Id => " + playerData.Id + ", Tag => " + playerData.GameTag + ", Team => " + playerData.PlayerTeam);

            GamePlayerInfo gamePlayerInfo = GetComponent<GamePlayerInfo>();
        }

        public void SetPlayerConnected()
        {
            MatchInfo.Instance.SetNumberOdPlayersInTeamsConnected(
                MatchInfo.Instance.NumberOfPlayersInTeamsConnected + 1
            );
        }

        public PlayerStats GetStats()
        {
            return new PlayerStats
            {
                MatchesPlayed = 1,
                Goals = goals,
                Touches = touches,
                SecondsPlayed = SecondsPlayed
            };
        }

        public RankingPlayerStats GetRankingStats()
        {
            return new RankingPlayerStats
            {
                PlayerName = TagName,
                MatchesPlayed = 1,
                Goals = goals,
                Touches = touches,
                SecondsPlayed = SecondsPlayed
            };
        }

        public PlayerMatchSummary GetSummary()
        {
            MatchResult matchResult = MatchResult.Draw;
            if (MatchInfo.Instance.GetLocalScore() > MatchInfo.Instance.GetVisitorScore() && Team == PlayerTeam.Local)
            {
                matchResult = MatchResult.Win;
            }
            else if (MatchInfo.Instance.GetLocalScore() < MatchInfo.Instance.GetVisitorScore() && Team == PlayerTeam.Visitor)
            {
                matchResult = MatchResult.Win;
            }
            else if(MatchInfo.Instance.GetLocalScore() < MatchInfo.Instance.GetVisitorScore() && Team == PlayerTeam.Local)
            {
                matchResult = MatchResult.Lose;
            }
            else if (MatchInfo.Instance.GetLocalScore() > MatchInfo.Instance.GetVisitorScore() && Team == PlayerTeam.Visitor)
            {
                matchResult = MatchResult.Lose;
            }
            else if (MatchInfo.Instance.GetLocalScore() == MatchInfo.Instance.GetVisitorScore())
            {
                matchResult = MatchResult.Draw; ;
            }
            return new PlayerMatchSummary
                {
                LocalScore = MatchInfo.Instance.GetLocalScore(),
                VisitorScore = MatchInfo.Instance.GetVisitorScore(),
                Result = matchResult
            };
        }
    }
}
