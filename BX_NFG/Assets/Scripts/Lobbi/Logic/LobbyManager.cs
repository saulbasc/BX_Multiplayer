using System;
using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Assets.Scripts.Core.FireB;
using Assets.Scripts.Core.Models;
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
            try
            {
                User user = await UserDAO.Instance.select(FirebaseActions.GetCurrentID());
                LobbyPlayerData playerData = new LobbyPlayerData(LobbyPlayersManager.Instance.GetLocalID(), user.Username);
                LobbyData lobbyData = new LobbyData();
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
            try
            {
                User user = await UserDAO.Instance.select(FirebaseActions.GetCurrentID());
                LobbyPlayerData playerData = new LobbyPlayerData(LobbyPlayersManager.Instance.GetLocalID(), user.Username);
                bool success = await LobbyServiceHandler.Instance.JoinLobby(code, playerData.Serialize());
                return success;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}