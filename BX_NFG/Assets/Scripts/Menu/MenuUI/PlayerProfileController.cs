using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu.MenuUI
{
    public class PlayerProfileController : MonoBehaviour
    {
        [SerializeField] private Image playerAvatar;
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private Button changeNameButton;
        [SerializeField] private Button backButton;

        private void OnEnable()
        {
            changeNameButton.onClick.AddListener(OnChangeNameButtonClicked);
            backButton.onClick.AddListener(OnBackButtonPressed);
        }

        private void OnDisable()
        {
            changeNameButton.onClick.RemoveListener(OnChangeNameButtonClicked);
            backButton.onClick.RemoveListener(OnBackButtonPressed);
        }

        private void OnBackButtonPressed()
        {
            gameObject.SetActive(false);
        }

        private void OnChangeNameButtonClicked()
        {
            
        }
    }
}
