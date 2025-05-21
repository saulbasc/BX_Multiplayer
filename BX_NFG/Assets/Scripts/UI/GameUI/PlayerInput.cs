using System.Collections;
using Assets.Scripts.Game.GameEvents.Player;
using Assets.Scripts.UI.GameUI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Input
{
    public class PlayerInput : NetworkBehaviour
    {
        private FloatingJoystick joystick;
        private Button shootButton;
        private Button passButton;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            StartCoroutine(SetupInput());
        }

        private IEnumerator SetupInput()
        {
            yield return new WaitForSeconds(0.5f); 

            joystick = FindAnyObjectByType<FloatingJoystick>();
            foreach (var button in FindObjectsByType<Button>(FindObjectsSortMode.None))
            {
                if (button.name == InputButton.ShootButton.ToString())
                {
                    shootButton = button;
                    shootButton.onClick.AddListener(OnShootButton);
                }
                else if (button.name == InputButton.PassButton.ToString())
                {
                    passButton = button;
                    passButton.onClick.AddListener(OnPassButton);
                }
            }
        }


        public override void OnNetworkDespawn()
        {
            if (shootButton != null)
                shootButton.onClick.RemoveAllListeners();

            if (passButton != null)
                passButton.onClick.RemoveAllListeners();
        }

        private void OnPassButton()
        {
            PlayerEvents.OnPassAction?.Invoke();
        }

        private void OnShootButton()
        {
            PlayerEvents.OnShootAction?.Invoke();
        }

        public Vector3 GetPlayerInput()
        {
            return joystick != null ? joystick.Direction : Vector3.zero;
        }
    }
}
