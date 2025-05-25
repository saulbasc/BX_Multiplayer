
namespace Assets.Scripts.Camera
{
    using Assets.Scripts.Game.GameEvents.Player;
    using Unity.Netcode;
    using UnityEngine;

    public class MatchCamera : MonoBehaviour
    {
        private Transform target;

        public Vector3 offset = new Vector3(0, 5, -7);
        public float followSpeed = 5f;

        void LateUpdate()
        {
            if (target == null)
            {
                foreach (var player in FindObjectsByType<PlayerInGame>(FindObjectsSortMode.None))
                {
                    if (player.IsOwner)
                    {
                        target = player.transform;
                        break;
                    }
                }
            }
            movement();
        }

        private void movement()
        {
            if ( target != null)
            {
                Vector3 desiredPosition = target.position + offset;
                transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
                transform.LookAt(target);
            }
        }
    }

}
