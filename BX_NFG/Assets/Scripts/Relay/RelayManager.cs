
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Assets.Scripts.Commons;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Assets.Scripts.Lobbi.Datas;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Lobbi.Logic
{
    public class RelayManager : Singleton<RelayManager>
    {
        private int maxPlayers = 10;

        private bool host = false;
        private string joinCode;
        private string ip;
        private int port;
        private byte[] connectionData;
        private byte[] key;
        private byte[] hostConnectionData;
        private byte[] allocationIdBytes;
        private Guid allocationId;

        public bool IsHost()
        {
            return host;
        }

        public string GetAllocatorId()
        {
            return allocationId.ToString();
        }

        public string GetConnectionData()
        {
            return connectionData.ToString();
        }

        public async Task<string> CreateRelay(int maxConnections)
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            var joincode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(connection => connection.ConnectionType == "dtls");
            ip = dtlsEndpoint.Host;
            port = dtlsEndpoint.Port;

            allocationId = allocation.AllocationId;
            allocationIdBytes = allocation.AllocationIdBytes;
            connectionData = allocation.ConnectionData;
            key = allocation.Key;
            
            host = true;

            return joincode;
        }

        public async Task<bool> JoinRelay(string joinCode)
        {
            this.joinCode = joinCode;
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);


            RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(connection => connection.ConnectionType == "dtls");
            ip = dtlsEndpoint.Host;
            port = dtlsEndpoint.Port;

            allocationId = allocation.AllocationId;
            allocationIdBytes = allocation.AllocationIdBytes;
            connectionData = allocation.ConnectionData;
            hostConnectionData = allocation.HostConnectionData;
            key = allocation.Key;

            return true;
        }

        public async Task StartRelayServer()
        {
            string relayCode = await RelayManager.Instance.CreateRelay(maxPlayers);
            PlayerStatus.Instance.InGame = true;

            LobbyDataManager.Instance.SetTotalPlayersInTeamsAsync();

            LobbyData lobbyData = new LobbyData(relayCode, "GameScene", MatchDuration.matchDuration1);
            await LobbyServiceHandler.Instance.UpdateLobbyData(lobbyData.Serialize());

            MatchInfo.Instance.AddNewPlayerConnectedServerRpc();
            MatchInfo.Instance.MatchDuration = LobbyDataManager.Instance.GetMatchDuration();

            string allocationId = GetAllocatorId();
            string connectionData = GetConnectionData();

            try
            {
                await LobbyPlayersManager.Instance.SetLocalPlayerData(allocationId, connectionData);
                await SceneManager.LoadSceneAsync("GameScene");
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
        }

        public async Task<bool> JoinRelayServer()
        {
            await JoinRelay(LobbyDataManager.Instance.GetRelayCode());
            PlayerStatus.Instance.InGame = true;

            MatchInfo.Instance.AddNewPlayerConnectedServerRpc();

            string allocationId = RelayManager.Instance.GetAllocatorId();
            string connectionData = RelayManager.Instance.GetConnectionData();

            try
            {
                await Task.Delay(200);
                await LobbyPlayersManager.Instance.SetLocalPlayerData(allocationId, connectionData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);  
            }
            return true;
        }

        public (byte[] allocationId, byte[] key, byte[] connectionData, string dtslAdrres, int dtlsPort) GetHostConnectionData()
        {
            return (allocationIdBytes, key, connectionData, ip, port);
        }

        public (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string dtslAdrres, int dtlsPort) GetClientConnectionData()
        {
            return (allocationIdBytes, key, connectionData, hostConnectionData, ip, port);
        }
    }
}
