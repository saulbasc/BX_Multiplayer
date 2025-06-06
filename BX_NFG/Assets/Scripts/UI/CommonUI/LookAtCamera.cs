using UnityEngine;

namespace Assets.Scripts.UI.CommonUI
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera mainCamera;
        private const float fixedYRotation = 0f;

        private void Update()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                if (mainCamera == null)
                    return;
            }

            Vector3 camEuler = mainCamera.transform.rotation.eulerAngles;
            Quaternion fixedRotation = Quaternion.Euler(camEuler.x, fixedYRotation, 0f);
            transform.rotation = fixedRotation;
        }
    }
}
