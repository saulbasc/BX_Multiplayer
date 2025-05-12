using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using Assets.Scripts.Commons;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Datas;
using Assets.Scripts.Handlers;
using Assets.Scripts.Core.FireB;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Lobbi.Players;

namespace Assets.Scripts.Lobbi.Logic
{
    public class LobbyServiceManager : DefaultSingleton<LobbyServiceManager>
    {
        /// <summary>
        /// Jugadores máximos para la Lobby y partido.
        /// </summary>
        int maxPlayers = 10;

        /// <summary>
        /// Crea una nueva Lobby y establece al usuario local como host.
        /// </summary>
        /// <returns>True si la creación se completa con éxtio.</returns>
        public async Task<bool> CreateLobby()
        {
            User user = await UserDAO.Instance.select(FirebaseActions.GetCurrentID());
            LobbyPlayerData rawPlayerData = new LobbyPlayerData(UnityServicesActions.GetCurrentUserID(), user.Username);
            Dictionary<string, string> dictionaryPlayerData = rawPlayerData.SerializeObjectToDictionary();
            Dictionary<string, string> dictionaryLobbyData = new LobbyData().SerializeObjectToDictionary();

            Dictionary<string, PlayerDataObject> playerDataObject = DataUtil.ToPlayerDataObjectDictionary(dictionaryPlayerData);
            Dictionary<string, DataObject> lobbyDataObject = DataUtil.ToLobbyDataObjectDictionary(dictionaryLobbyData);
            Player player = new Player(UnityServicesActions.GetCurrentUserID(), null, playerDataObject);

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = player,
                Data = lobbyDataObject,
            };

            return await SafeAsyncFunctionsHandler.ExecuteAsync( async () =>
            {
                Lobby newLobby = await LobbyService.Instance.CreateLobbyAsync("MyLobby", maxPlayers, lobbyOptions);
                LobbyDataManager.Instance.SetLobby(newLobby);
                LobbyCoroutineManager.Instance.StartUpdateLobbyCororutine(LobbyDataManager.Instance.GetLobbyID(), 1f);
                LobbyCoroutineManager.Instance.StartHeartbeatCororutine(LobbyDataManager.Instance.GetLobbyID(), 5f);
                return true;
            }, false);
        }

        /// <summary>
        /// Se une a una Lobby ya existente con su código.
        /// </summary>
        /// <param name="code">El código de la Lobby a unirse.</param>
        /// <returns>True si se une a la Lobby con éxito.</returns>
        public async Task<bool> JoinLobby(string code)
        {
            User user = await UserDAO.Instance.select(FirebaseActions.GetCurrentID());
            LobbyPlayerData rawPlayerData = new LobbyPlayerData(UnityServicesActions.GetCurrentUserID(), user.Username);
            Dictionary<string, string> serializedRawPlayerData = rawPlayerData.SerializeObjectToDictionary();
            Dictionary<string, PlayerDataObject> playerData = DataUtil.ToPlayerDataObjectDictionary(serializedRawPlayerData);
            Player player = new Player(UnityServicesActions.GetCurrentUserID(), null, playerData);

            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions
            {
                Player = player,
            };

            return await SafeAsyncFunctionsHandler.ExecuteAsync( async () =>
            {
                Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
                LobbyDataManager.Instance.SetLobby(lobby);
                LobbyCoroutineManager.Instance.StartUpdateLobbyCororutine(LobbyDataManager.Instance.GetLobbyID(), 1f);
                return true;
            }, false);
        }

        /// <summary>
        /// El usuario se desconecta de la Lobby en la que está y destruye las instancias de esta.
        /// </summary>
        /// <returns>True si la desconexión se ha producido con éxito.</returns>
        public async Task<bool> DisconnectFromLobby()
        {
            return await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                if (UnityServicesActions.GetCurrentUserID() == LobbyDataManager.Instance.GetHostID())
                {
                    await HostDisconnection();
                }
                else
                {
                    await LobbyService.Instance.RemovePlayerAsync(LobbyDataManager.Instance.GetLobbyID(), UnityServicesActions.GetCurrentUserID());
                }
                DestroyAllLobbyInstances();
                await SceneManager.LoadSceneAsync(Scenes.MenuScene.ToString());
                return true;
            }, false);
        }

        private async Task HostDisconnection()
        {
            List<Player> players = LobbyDataManager.Instance.GetPlayers();
            Player newHost = players.FirstOrDefault(player => player.Id != UnityServicesActions.GetCurrentUserID());
            await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                if (newHost != null)
                {
                    await LobbyService.Instance.UpdateLobbyAsync(LobbyDataManager.Instance.GetLobbyID(), new UpdateLobbyOptions
                    {
                        HostId = newHost.Id
                    });
                    await LobbyService.Instance.RemovePlayerAsync(LobbyDataManager.Instance.GetLobbyID(), UnityServicesActions.GetCurrentUserID());   
                }
                else
                {
                    await LobbyService.Instance.DeleteLobbyAsync(LobbyDataManager.Instance.GetLobbyID());
                }
            });
        }

        private void DestroyAllLobbyInstances()
        {
            try
            {
                LobbyUpdaterManager.Instance.Delete();
                LobbyCoroutineManager.Instance.Delete();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}