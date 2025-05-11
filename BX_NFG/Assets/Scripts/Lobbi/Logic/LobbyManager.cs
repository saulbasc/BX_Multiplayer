using System;
using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Assets.Scripts.Core.FireB;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Datas;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.Lobbi.Players;
using UnityEngine;

namespace Assets.Scripts.Connection.Lobbi
{
    /// <summary>
    /// Gestiona la creación y unión a las salas.
    /// </summary>
    public class LobbyManager : Singleton<LobbyManager>
    {
        int maxPlayers = 10;

        /// <summary>
        /// Crea una nueva sala de partida con el host como ID local.
        /// </summary>
        /// <returns>True si se crea exitosamente la sala.</returns>
        public async Task<bool> CreateLobby()
        {
            try
            {
                User user = await UserDAO.Instance.select(FirebaseActions.GetCurrentID());
                LobbyPlayerData playerData = new LobbyPlayerData(UnityServicesActions.GetCurrentUserID(), user.Username);
                LobbyData lobbyData = new LobbyData();
                bool success = await LobbyServiceHandler.Instance.CreateLobby(
                    maxPlayers, 
                    false, 
                    playerData.SerializeObjectToDictionary(), 
                    lobbyData.SerializeObjectToDictionary()
                );
                return success;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// Se une a una sala ya creada mediante el código de esta.
        /// </summary>
        /// <param name="code">El código de la sala a unirse.</param>
        /// <returns>True si el usuario se une a la sala correctamente</returns>
        public async Task<bool> JoinLobby(string code)
        {
            try
            {
                User user = await UserDAO.Instance.select(FirebaseActions.GetCurrentID());
                LobbyPlayerData playerData = new LobbyPlayerData(UnityServicesActions.GetCurrentUserID(), user.Username);
                bool success = await LobbyServiceHandler.Instance.JoinLobby(code, playerData.SerializeObjectToDictionary());
                return success;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// Elimina la instancia de la clase como gameObject.
        /// </summary>
        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}