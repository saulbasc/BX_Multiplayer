
namespace Assets.Scripts.MatchCamera
{
    using Assets.Scripts.Game.GameEvents.Player;
    using UnityEngine;

    public class GameMatchCamera : MonoBehaviour
    {
        private Transform target;

        float zoom;
        float angle;

        private const string ZoomKey = "CameraZoom";
        private const string AngleKey = "CameraAngle";

        private bool targetAsigned;

        private void Awake()
        {
            GetSettings();
        }

        private void GetSettings()
        {
            if (PlayerPrefs.HasKey(ZoomKey))
            {
                zoom = PlayerPrefs.GetFloat(ZoomKey);
            }
            else
            {
                zoom = 3f;
            }
            if (PlayerPrefs.HasKey(AngleKey))
            {
                angle = PlayerPrefs.GetFloat(AngleKey);
            }
            else
            {
                angle = 0.5f;
            }
        }

        void LateUpdate()
        {
            if (target == null || !targetAsigned)
            {
                target = GameObject.FindGameObjectWithTag("Ball")?.transform;
                if (target != null)
                {
                    targetAsigned = true;
                }
            }

            if (target != null)
            {
                UpdateCameraPosition();
            }
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            targetAsigned = true;
        }

        private void UpdateCameraPosition()
        {
            float zoom = this.zoom * 10f; 
            float angleX = Mathf.Lerp(90f, 45f, angle); 

            float rad = Mathf.Deg2Rad * angleX;

            float y = Mathf.Sin(rad) * zoom;
            float z = -Mathf.Cos(rad) * zoom;

            Vector3 offset = new Vector3(0, y, z);
            transform.position = target.transform.position + offset;

            transform.LookAt(target.transform.position);
        }
    }
}
