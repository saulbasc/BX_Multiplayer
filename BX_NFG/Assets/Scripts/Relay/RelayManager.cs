
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
        private string joinCode;
        private string ip;
        private int port;
        private byte[] connectionData;
        private Guid allocationId;

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
            connectionData = allocation.ConnectionData;

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
            connectionData = allocation.ConnectionData;

            return true;
        }
    }
}
