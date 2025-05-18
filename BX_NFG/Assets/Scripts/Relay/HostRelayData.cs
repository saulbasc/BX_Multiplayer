using System;

public class HostRelayData
{
    public string Ip { get; private set; }
    public int Port { get; private set; }
    public byte[] ConnectionData { get; private set; }
    public byte[] Key { get; private set; }
    public byte[] AllocationIdBytes { get; private set; }
    public Guid AllocationId { get; private set; }

    public HostRelayData(string ip, int port, byte[] connectionData, byte[] key, byte[] allocationIdBytes, Guid allocationId)
    {
        Ip = ip;
        Port = port;
        ConnectionData = connectionData;
        Key = key;
        AllocationIdBytes = allocationIdBytes;
        AllocationId = allocationId;
    }

    public (byte[] allocationId, byte[] key, byte[] connectionData, string ip, int port) GetConnectionData()
    {
        return (AllocationIdBytes, Key, ConnectionData, Ip, Port);
    }
}
