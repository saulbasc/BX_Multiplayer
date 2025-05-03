using System;
using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Assets.Scripts.Lobbi.Datas;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Lobbi.Players;
using UnityEngine;

namespace Assets.Scripts.Connection.Lobbi
{
    public class GamePlayersManager : Singleton<GamePlayersManager>
    {
        int maxPlayers = 10;

        public async Task<bool> CreateLobby()
        {
            LobbyPlayerData playerData = new LobbyPlayerData(LobbyPlayersManager.Instance.GetLocalID(), "HostPlayer");
            LobbyData lobbyData = new LobbyData();
            try
            {
                bool success = await LobbyServiceHandler.Instance.CreateLobby(maxPlayers, false, playerData.Serialize(), lobbyData.Serialize());
                return success;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public async Task<bool> JoinLobby(string code)
        {
            LobbyPlayerData playerData = new LobbyPlayerData(LobbyPlayersManager.Instance.GetLocalID(), "JoinPlayer");
            try
            {
                bool success = await LobbyServiceHandler.Instance.JoinLobby(code, playerData.Serialize());
                return success;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}