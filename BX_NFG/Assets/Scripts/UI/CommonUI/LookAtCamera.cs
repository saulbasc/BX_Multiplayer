using UnityEngine;

namespace Assets.Scripts.UI.CommonUI
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if(mainCamera != null)
            {
                transform.LookAt(mainCamera.transform);
                transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
            }
            else
            {
                if(Camera.main != null)
                {
                    mainCamera = Camera.main;
                }
            }
        }
    }
}
