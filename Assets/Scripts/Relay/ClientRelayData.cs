using System;

public class ClientRelayData
{
    public string Ip { get; private set; }
    public int Port { get; private set; }
    public byte[] ConnectionData { get; private set; }
    public byte[] Key { get; private set; }
    public byte[] HostConnectionData { get; private set; }
    public byte[] AllocationIdBytes { get; private set; }
    public Guid AllocationId { get; private set; }

    public ClientRelayData(string ip, int port, byte[] connectionData, byte[] key, byte[] hostConnectionData, byte[] allocationIdBytes, Guid allocationId)
    {
        Ip = ip;
        Port = port;
        ConnectionData = connectionData;
        Key = key;
        HostConnectionData = hostConnectionData;
        AllocationIdBytes = allocationIdBytes;
        AllocationId = allocationId;
    }

    public (byte[] allocationId, byte[] key, byte[] connectionData, byte[] hostConnectionData, string dtslAdrres, int dtlsPort) GetConnectionData()
    {
        return (AllocationIdBytes, Key, ConnectionData, HostConnectionData, Ip, Port);
    }
}