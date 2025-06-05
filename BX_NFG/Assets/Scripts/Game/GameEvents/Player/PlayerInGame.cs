using System;
using System.Collections;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Game.GameEvents.Spawner;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Lobbi.Players;
using Assets.Scripts.MatchCamera;
using Assets.Scripts.UI.MenuUI.Components;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Game.GameEvents.Player
{
    public class PlayerInGame : NetworkBehaviour
    {
        [SerializeField] private GameObject cameraPrefab;

        private MatchInfo matchInfo;
        private LobbyPlayerManager lobbyPlayerManager;

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
            StartCoroutine(WaitForMatchInfo());
        }

        private void Update()
        {
            if (!IsServer) return;
            AddTime(Time.deltaTime);
        }

        private IEnumerator WaitForMatchInfo()
        {
            if (IsServer)
            {
                GameObject manager = null;
                while (manager == null)
                {
                    manager = GameObject.Find("GameManager");
                    yield return null;
                }

                while (lobbyPlayerManager == null)
                {
                    lobbyPlayerManager = FindFirstObjectByType<LobbyPlayerManager>();
                    if (lobbyPlayerManager == null)
                    {
                        yield return null;
                    }
                }

                matchInfo = manager.GetComponent<MatchInfo>();

                if (matchInfo != null)
                {
                    SetPlayerConnected();
                }
            }

            if (IsOwner)
            {
                SetCamera();
                string userId = UnityServicesActions.GetCurrentUserID();
                SendDataServerRpc(userId);
            }
        }

        private void SetCamera()
        {
            if (cameraPrefab == null) return;

            GameObject cameraInstance = Instantiate(cameraPrefab);
            var matchCamera = cameraInstance.GetComponent<GameMatchCamera>();
            matchCamera.SetTarget(transform);
        }

        public static event Action<PlayerInGame> OnPlayerDataInitialized;

        [ServerRpc(RequireOwnership = true)]
        public void SendDataServerRpc(string userId, ServerRpcParams rpcParams = default)
        {
            LobbyPlayerData playerData = lobbyPlayerManager.GetSinglePlayerDataObject(userId);
            if(playerData.PlayerTeam == PlayerTeam.Spectator)  return;

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
            matchInfo.SetNumberOdPlayersInTeamsConnected(
                matchInfo.NumberOfPlayersInTeamsConnected + 1
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
            if (matchInfo.GetLocalScore() > matchInfo.GetVisitorScore() && Team == PlayerTeam.Local)
            {
                matchResult = MatchResult.Win;
            }
            else if (matchInfo.GetLocalScore() < matchInfo.GetVisitorScore() && Team == PlayerTeam.Visitor)
            {
                matchResult = MatchResult.Win;
            }
            else if(matchInfo.GetLocalScore() < matchInfo.GetVisitorScore() && Team == PlayerTeam.Local)
            {
                matchResult = MatchResult.Lose;
            }
            else if (matchInfo.GetLocalScore() > matchInfo.GetVisitorScore() && Team == PlayerTeam.Visitor)
            {
                matchResult = MatchResult.Lose;
            }
            else if (matchInfo.GetLocalScore() == matchInfo.GetVisitorScore())
            {
                matchResult = MatchResult.Draw; ;
            }
            return new PlayerMatchSummary
                {
                LocalScore = matchInfo.GetLocalScore(),
                VisitorScore = matchInfo.GetVisitorScore(),
                Result = matchResult    
            };
        }
    }
}
