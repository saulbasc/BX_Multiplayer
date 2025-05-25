using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Input
{
    public class GameJoystick : NetworkBehaviour
    {
        private FloatingJoystick joystick;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            StartCoroutine(SetupInput());
        }

        private IEnumerator SetupInput()
        {
            yield return new WaitForSeconds(0.5f); 
            joystick = FindAnyObjectByType<FloatingJoystick>();
        }

        public Vector3 GetPlayerInput()
        {
            return joystick != null ? joystick.Direction : Vector3.zero;
        }
    }
}
