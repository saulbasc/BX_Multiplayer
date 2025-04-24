
using System;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Commons;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

namespace Assets.Scripts.Lobbi.Logic
{
    public class RelayManager : Singleton<RelayManager>
    {
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
