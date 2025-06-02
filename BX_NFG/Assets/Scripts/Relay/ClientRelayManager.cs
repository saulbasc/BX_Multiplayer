using System;
using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Logic;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using System.Linq;
using Assets.Scripts.Handlers;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Assets.Scripts.Lobbi.Players;
using Assets.Scripts.Lobbi.Data;

namespace Assets.Scripts.Relay
{
    public class ClientRelayManager : Singleton<ClientRelayManager>
    {
        private ClientRelayData clientRelayData;
        public string GetAllocatorId() => clientRelayData.AllocationId.ToString();
        public string GetConnectionData() => Convert.ToBase64String(clientRelayData.ConnectionData);

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public async Task<bool> JoinRelayServer()
        {
            return await SafeAsyncFunctionsHandler.ExecuteAsync(async () =>
            {
                string code = LobbyDataManager.Instance.GetLobbyRelayCode();
                LobbyPlayerData lpd = LobbyPlayerManager.Instance.GetSinglePlayerDataObject(UnityServicesActions.GetCurrentUserID());

                JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(code);
                RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(connection => connection.ConnectionType == "udp");

                clientRelayData = new ClientRelayData(
                    dtlsEndpoint.Host, dtlsEndpoint.Port, allocation.ConnectionData, allocation.Key,
                    allocation.HostConnectionData, allocation.AllocationIdBytes, allocation.AllocationId
                );

                PlayerStatus.Instance.InGame = true;
                await Task.Delay(200);

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                    clientRelayData.Ip,
                    (ushort)clientRelayData.Port,
                    clientRelayData.AllocationIdBytes,
                    clientRelayData.Key,
                    clientRelayData.ConnectionData,
                    clientRelayData.HostConnectionData
                );

                await Task.Delay(500);

                await LobbyPlayerManager.Instance.UpdatePlayerOptions(UnityServicesActions.GetCurrentUserID(), GetAllocatorId(), GetConnectionData());
                NetworkManager.Singleton.StartClient();
                return true;
            }, false);
        }

        public (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string ip, int port) GetClientConnectionData()
        {
            return clientRelayData.GetConnectionData();
        }
    }
}
