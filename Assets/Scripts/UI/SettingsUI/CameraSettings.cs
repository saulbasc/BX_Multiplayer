using System;
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
        [SerializeField] private Button exitButton;

        private const string ZoomKey = "CameraZoom";
        private const string AngleKey = "CameraAngle";

        private void OnEnable()
        {
            exitButton.onClick.AddListener(OnExit);
            saveButton.onClick.AddListener(SaveSettings);
            cameraZoom.minValue = 2f;
            cameraZoom.maxValue = 7f;
            cameraAngle.onValueChanged.AddListener(OnAngleChanged);
            cameraZoom.onValueChanged.AddListener(OnZoomChanged);
            GetSettings();
        }

        private void OnDisable()
        {
            exitButton.onClick.RemoveListener(OnExit);
            saveButton.onClick.RemoveListener(SaveSettings);
            cameraAngle.onValueChanged.RemoveListener(OnAngleChanged);
            cameraZoom.onValueChanged.RemoveListener(OnZoomChanged);
        }
        private void UpdateCameraPosition()
        {
            float zoom = cameraZoom.value * 10f; 
            float angleX = Mathf.Lerp(90f, 45f, cameraAngle.value); 

            float rad = Mathf.Deg2Rad * angleX;

            float y = Mathf.Sin(rad) * zoom;
            float z = -Mathf.Cos(rad) * zoom; 

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

        private void OnExit()
        {
            SceneManager.LoadScene(Scenes.MenuScene.ToString());
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
