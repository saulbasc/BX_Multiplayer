using System;
using System.Collections.Generic;
using Assets.Scripts.Connection.Lobbi;
using UnityEngine;

namespace Assets.Scripts.Lobbi
{
    public class LobbySpawner : MonoBehaviour
    {
        [SerializeField] private List<LobbyPlayer> players;

        private void OnEnable()
        {
            GameLobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            GameLobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        }

        private void OnLobbyUpdated()
        {
            List<LobbyPlayerData> playerDataList = GameLobbyManager.Instance.GetPlayerDataList();

            for(int i = 0; i < playerDataList.Count; i++)
            {
                LobbyPlayerData playerData = playerDataList[i];
                players[i].SetPlayerData(playerData);
            }
        }
    }
}
