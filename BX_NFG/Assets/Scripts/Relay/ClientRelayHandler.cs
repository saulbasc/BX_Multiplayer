using System.Threading.Tasks;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using System.Linq;
using System;

public class ClientRelayHandler
{
    public string Ip { get; private set; }
    public int Port { get; private set; }
    public byte[] ConnectionData { get; private set; }
    public byte[] Key { get; private set; }
    public byte[] HostConnectionData { get; private set; }
    public byte[] AllocationIdBytes { get; private set; }
    public Guid AllocationId { get; private set; }

    public async Task JoinRelayAsync(string joinCode)
    {
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(connection => connection.ConnectionType == "dtls");
        Ip = dtlsEndpoint.Host;
        Port = dtlsEndpoint.Port;

        AllocationId = allocation.AllocationId;
        AllocationIdBytes = allocation.AllocationIdBytes;
        ConnectionData = allocation.ConnectionData;
        HostConnectionData = allocation.HostConnectionData;
        Key = allocation.Key;
    }

    public (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string dtslAdrres, int dtlsPort) GetConnectionData()
    {
        return (AllocationIdBytes, Key, ConnectionData, HostConnectionData, Ip, Port);
    }
}