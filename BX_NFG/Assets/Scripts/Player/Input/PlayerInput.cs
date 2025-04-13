using UnityEngine;

namespace Assets.Scripts.Input
{
    public class PlayerInput : MonoBehaviour
    {

        private FloatingJoystick joystick;

        private void Awake()
        {
            joystick = FindAnyObjectByType<FloatingJoystick>();
        }

        public Vector3 GetPlayerInput()
        {
            return joystick.Direction;
        }
    }
}
