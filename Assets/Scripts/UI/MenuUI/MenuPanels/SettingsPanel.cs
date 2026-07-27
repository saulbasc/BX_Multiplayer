using Assets.Scripts.Commons;
using Assets.Scripts.UI.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI
{
    public class SettingsPanel : PanelBase
    {
        public override PanelType PanelType => PanelType.SettingsPanel;
        [SerializeField] private Button backButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button cameraSettingsButton;

        public override void Initialize(IUIManager manager)
        {
            base.manager = manager;

            backButton.onClick.AddListener(() => base.manager.RemoveFloatPanel(PanelType));
            saveButton.onClick.AddListener(() => base.manager.RemoveFloatPanel(PanelType));
            cameraSettingsButton.onClick.AddListener(() => SceneManager.LoadScene(Scenes.SettingsScene.ToString()));
        }
    }
}
