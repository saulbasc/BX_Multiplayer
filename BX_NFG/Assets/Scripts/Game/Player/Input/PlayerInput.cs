using System;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public class PlayerInput : NetworkBehaviour
    {
        private FloatingJoystick joystick;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            joystick = FindAnyObjectByType<FloatingJoystick>();
        }

        public Vector3 GetPlayerInput()
        {
            return joystick != null ? joystick.Direction : Vector3.zero;
        }
    }
}
