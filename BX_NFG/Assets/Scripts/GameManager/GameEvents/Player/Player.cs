using System;
using Unity.Netcode;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.GameManager.GameEvents.Player
{
    [Serializable]
    public class Player
    {
        public PlayerRole PlayerRoles { get; private set; }

        public float Speed { get; private set; }
        public float Strenght { get; private set; }
        public float Power { get; private set; }

        public NetworkObject NetworkObject { get; private set; }

        public Player (
            NetworkObject networkObject, 
            PlayerRole playerRoles, 
            float speed, 
            float strenght, 
            float power)
        {
            NetworkObject = networkObject;
            PlayerRoles = playerRoles;
            Speed = speed;
            Strenght = strenght;
            Power = power;
        }
    }
}
