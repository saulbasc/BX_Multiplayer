using Assets.Scripts.Commons;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.SettingsUI
{
    public class CameraSettings : MonoBehaviour
    {
        [SerializeField] private GameObject settingsCamera;
        [SerializeField] private GameObject player;
        [SerializeField] private Slider cameraZoom;
        [SerializeField] private Slider cameraAngle;
        [SerializeField] private Button saveButton;

        private const string ZoomKey = "CameraZoom";
        private const string AngleKey = "CameraAngle";

        private void OnEnable()
        {
            saveButton.onClick.AddListener(SaveSettings);
            cameraZoom.minValue = 2f;
            cameraZoom.maxValue = 5f;
            cameraAngle.onValueChanged.AddListener(OnAngleChanged);
            cameraZoom.onValueChanged.AddListener(OnZoomChanged);
            GetSettings();
        }

        private void OnDisable()
        {
            saveButton.onClick.RemoveListener(SaveSettings);
            cameraAngle.onValueChanged.RemoveListener(OnAngleChanged);
            cameraZoom.onValueChanged.RemoveListener(OnZoomChanged);
        }
        private void UpdateCameraPosition()
        {
            float zoom = cameraZoom.value * 10f; // distancia desde el jugador
            float angleX = Mathf.Lerp(90f, 45f, cameraAngle.value); // de top-down a 45°

            // Convertimos el ángulo a radianes para trigonometría
            float rad = Mathf.Deg2Rad * angleX;

            // Calculamos offset en un arco vertical
            float y = Mathf.Sin(rad) * zoom;
            float z = -Mathf.Cos(rad) * zoom; // hacia atrás

            Vector3 offset = new Vector3(0, y, z);
            settingsCamera.transform.position = player.transform.position + offset;

            settingsCamera.transform.LookAt(player.transform.position);
        }

        private void OnZoomChanged(float value)
        {
            UpdateCameraPosition();
        }

        private void OnAngleChanged(float value)
        {
            UpdateCameraPosition();
        }



        private void GetSettings()
        {
            if (PlayerPrefs.HasKey(ZoomKey))
            {
                cameraZoom.value = PlayerPrefs.GetFloat(ZoomKey);
            }
            if (PlayerPrefs.HasKey(AngleKey))
            {
                cameraAngle.value = PlayerPrefs.GetFloat(AngleKey);
            }
        }

        private void SaveSettings()
        {
            PlayerPrefs.SetFloat(ZoomKey, cameraZoom.value);
            PlayerPrefs.SetFloat(AngleKey, cameraAngle.value);
            PlayerPrefs.Save();
            SceneManager.LoadScene(Scenes.MenuScene.ToString());
        }
    }
}
