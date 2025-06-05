using System;
using Assets.Scripts.Lobbi.Data;
using Unity.Collections;
using Unity.Netcode;

namespace Assets.Scripts.Game.GameEvents.Player
{
    public struct FinalPlayerStatsData : INetworkSerializable, IEquatable<FinalPlayerStatsData>
    {
        public FixedString32Bytes PlayerName;
        public PlayerTeam PlayerTeam;
        public int Goals;
        public int Touches;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref PlayerName);
            serializer.SerializeValue(ref PlayerTeam);
            serializer.SerializeValue(ref Goals);
            serializer.SerializeValue(ref Touches);
        }

        public bool Equals(FinalPlayerStatsData other)
        {
            return PlayerName.Equals(other.PlayerName) &&
                   PlayerTeam == other.PlayerTeam &&
                   Goals == other.Goals &&
                   Touches == other.Touches;
        }

        public override bool Equals(object obj)
        {
            return obj is FinalPlayerStatsData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PlayerName, PlayerTeam, Goals, Touches);
        }
    }
}
