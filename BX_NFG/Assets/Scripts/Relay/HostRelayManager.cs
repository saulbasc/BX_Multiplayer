
using Assets.Scripts.Commons;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Datas;
using Assets.Scripts.Lobbi.Logic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Relay
{
    public class HostRelayManager : Singleton<HostRelayManager>
    {
        private HostRelayHandler relayHandler;

        public string GetAllocatorId() => relayHandler.AllocationId.ToString();
        public string GetConnectionData() => Convert.ToBase64String(relayHandler.ConnectionData);

        private void Awake()
        {
            relayHandler = new HostRelayHandler();
        }

        public async Task StartRelayServer()
        {
            try
            {
                string joinCode = await relayHandler.CreateRelayAsync();

                PlayerStatus.Instance.InGame = true;
                LobbyDataManager.Instance.SetTotalPlayersInTeamsInMatchInfo();

                LobbyData actualLobbyData = LobbyDataManager.Instance.GetLobbyDataObject();
                var lobbyData = new LobbyData(joinCode, actualLobbyData.MatchDuration);
                MatchInfo.Instance.SetMatchDuration(actualLobbyData.MatchDuration);
                await LobbyDataManager.Instance.UpdateLobbyData(lobbyData.SerializeObjectToDictionary());

                MatchInfo.Instance.SetMatchDuration(LobbyDataManager.Instance.GetLobbyMatchDuration());

                await LobbyPlayersManager.Instance.UpdatePlayerOptions(UnityServicesActions.GetCurrentUserID(), GetAllocatorId(), GetConnectionData());

                await SceneManager.LoadSceneAsync(Scenes.GameScene.ToString());
            }
            catch (Exception e)
            {
                Debug.LogError($"Error starting relay server: {e}");
            }
        }

        public (byte[] allocationId, byte[] key, byte[] connectionData, string ip, int port) GetHostConnectionData()
        {
            return relayHandler.GetConnectionData();
        }
    }
}
