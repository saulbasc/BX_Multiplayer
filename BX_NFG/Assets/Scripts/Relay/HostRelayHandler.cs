using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class HostRelayHandler
{
    public string Ip { get; private set; }
    public int Port { get; private set; }
    public byte[] ConnectionData { get; private set; }
    public byte[] Key { get; private set; }
    public byte[] AllocationIdBytes { get; private set; }
    public Guid AllocationId { get; private set; }
    private int maxConnections = 10;
  
    public async Task<string> CreateRelayAsync()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        var joincode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(connection => connection.ConnectionType == "dtls");
        Ip = dtlsEndpoint.Host;
        Port = dtlsEndpoint.Port;

        AllocationId = allocation.AllocationId;
        AllocationIdBytes = allocation.AllocationIdBytes;
        ConnectionData = allocation.ConnectionData;
        Key = allocation.Key;

        return joincode;
    }

    public (byte[] allocationId, byte[] key, byte[] connectionData, string ip, int port) GetConnectionData()
    {
        return (AllocationIdBytes, Key, ConnectionData, Ip, Port);
    }
}
