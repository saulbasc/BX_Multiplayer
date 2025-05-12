using System;
using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Logic;
using UnityEngine;

namespace Assets.Scripts.Relay
{
    public class ClientRelayManager : Singleton<ClientRelayManager>
    {
        private ClientRelayHandler relayHandler;
        public string GetAllocatorId() => relayHandler.AllocationId.ToString();
        public string GetConnectionData() => Convert.ToBase64String(relayHandler.ConnectionData);

        private void Awake()
        {
            relayHandler = new ClientRelayHandler();
        }

        public async Task<bool> JoinRelayServer()
        {
            try
            {
                string code = LobbyDataManager.Instance.GetLobbyRelayCode();
                await relayHandler.JoinRelayAsync(code);

                PlayerStatus.Instance.InGame = true;

                await Task.Delay(200);

                await LobbyPlayerManager.Instance.UpdatePlayerOptions(UnityServicesActions.GetCurrentUserID(), GetAllocatorId(), GetConnectionData());

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error joining relay server: {e}");
                return false;
            }
        }

        public (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string ip, int port) GetClientConnectionData()
        {
            return relayHandler.GetConnectionData();
        }
    }
}
