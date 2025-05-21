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
using UnityEngine.SceneManagement;

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

                Debug.Log("ClientRelayManager: JoinRelayServer => " + code);
                JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(code);
                RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(connection => connection.ConnectionType == "dtls");

                clientRelayData = new ClientRelayData(
                    dtlsEndpoint.Host, dtlsEndpoint.Port, allocation.ConnectionData, allocation.Key,
                    allocation.HostConnectionData, allocation.AllocationIdBytes, allocation.AllocationId
                );

                PlayerStatus.Instance.InGame = true;
                await Task.Delay(200);
                

                Debug.Log("ClientRelayManager: JoinRelayServer => " +
                    $"IP: {clientRelayData.Ip}, Port: {clientRelayData.Port}, " +
                    $"AllocationId: {clientRelayData.AllocationId}, Key: {clientRelayData.Key}, " +
                    $"ConnectionData: {clientRelayData.ConnectionData}, HostConnectionData: {clientRelayData.HostConnectionData}"
                );

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
                    clientRelayData.Ip,
                    (ushort)clientRelayData.Port,
                    clientRelayData.AllocationIdBytes,
                    clientRelayData.Key,
                    clientRelayData.ConnectionData,
                    clientRelayData.HostConnectionData
                );

                await Task.Delay(500);

                Debug.Log("Cliente conectado: " + NetworkManager.Singleton.IsClient);
                Debug.Log("Network manager => " + NetworkManager.Singleton.name);
                NetworkManager.Singleton.StartClient();
                await LobbyPlayerManager.Instance.UpdatePlayerOptions(UnityServicesActions.GetCurrentUserID(), GetAllocatorId(), GetConnectionData());
                await SceneManager.LoadSceneAsync(Scenes.GameScene.ToString());
                return true;
            }, false);
        }

        public (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string ip, int port) GetClientConnectionData()
        {
            return clientRelayData.GetConnectionData();
        }
    }
}
