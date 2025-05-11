using Assets.Scripts.Init;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InitUI
{
    /// <summary>
    /// Gestiona la interfaz para que el usuario pueda poner su nuevo nombre al inicio
    /// </summary>
    public class NewNamePanel : MonoBehaviour
    {
        /// <summary>
        /// El panel para que el usuario introduzca su nombre.
        /// </summary>
        [SerializeField] private GameObject newUserNamePanel;
        /// <summary>
        /// El nombre que introduce el usuario.
        /// </summary>
        [SerializeField] private TMP_InputField newUserNameInput;
        /// <summary>
        /// Botón que utiliza el usuario para confirmar su nombre.
        /// </summary>
        [SerializeField] private Button confirmUserNameButton;

        private void Awake()
        {
            InitEventManager.Instance.OnUserNotRegistered += OnUserNotRegistered;
            InitEventManager.Instance.OnUserRegisteredSuccessfully += OnUserRegisteredSuccessfully;
            confirmUserNameButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        private void OnDestroy()
        {
            InitEventManager.Instance.OnUserNotRegistered -= OnUserNotRegistered;
            InitEventManager.Instance.OnUserRegisteredSuccessfully -= OnUserRegisteredSuccessfully;
            confirmUserNameButton.onClick.RemoveAllListeners();
        }

        private void OnUserRegisteredSuccessfully()
        {
            newUserNamePanel.SetActive(false);
        }

        private void OnUserNotRegistered()
        {
            newUserNamePanel.SetActive(true);
        }

        private void OnConfirmButtonClicked()
        {
            InitUIEvents.Instance.RaiserConfirmButtonClicked(newUserNameInput.text);
        }
    }
}
