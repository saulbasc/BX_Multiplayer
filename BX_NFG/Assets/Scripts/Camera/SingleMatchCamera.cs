
using Assets.Scripts.Game.GameEvents.Player;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class SingleMatchCamera : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        float zoom;
        float angle;

        private const string ZoomKey = "CameraZoom";
        private const string AngleKey = "CameraAngle";

        private void Awake()
        {
            GetSettings();
        }

        void LateUpdate()
        {
            UpdateCameraPosition();
        }

        private void GetSettings()
        {
            if (PlayerPrefs.HasKey(ZoomKey))
            {
                zoom = PlayerPrefs.GetFloat(ZoomKey);
            }
            if (PlayerPrefs.HasKey(AngleKey))
            {
                angle = PlayerPrefs.GetFloat(AngleKey);
            }
        }

        private void UpdateCameraPosition()
        {
            float zoom = this.zoom * 10f;
            float angleX = Mathf.Lerp(90f, 45f, angle);

            float rad = Mathf.Deg2Rad * angleX;

            float y = Mathf.Sin(rad) * zoom;
            float z = -Mathf.Cos(rad) * zoom;

            Vector3 offset = new Vector3(0, y, z);
            transform.position = player.transform.position + offset;

            transform.LookAt(player.transform.position);
        }
    }
}
