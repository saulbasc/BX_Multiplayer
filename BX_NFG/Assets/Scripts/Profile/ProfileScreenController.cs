using System;
using Assets.Scripts.Core.FireB;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Menu.MenuUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Profile
{
    public class ProfileScreenController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI profileName;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button changeName;
        [SerializeField] private Button confirmName;
        [SerializeField] private GameObject changeNamePanel;

        private void OnEnable()
        {
            changeName.onClick.AddListener(OnChangeNameButtonClicked);
            confirmName.onClick.AddListener(OnConfirmNameButtonClicked);
        }

        private void OnDisable()
        {
            changeName.onClick.RemoveAllListeners();
            confirmName.onClick.RemoveAllListeners();
        }

        private void OnChangeNameButtonClicked()
        {
            changeNamePanel.SetActive(true);
            CommonAnimations.ShowPanel(changeNamePanel);
        }

        private void OnConfirmNameButtonClicked()
        {
            changeNamePlayer(inputField.text);
            setNamePlayer();
        }

        private void Start()
        {
            setNamePlayer();
        }

        private async void setNamePlayer()
        {
            User user = await UserDAO.Instance.select(FirebaseActions.GetCurrentID());
            if (user != null)
            {
                profileName.text = user.Username;
            }
        }

        private async void changeNamePlayer(string newName)
        {
            User updateUser = new User(FirebaseActions.GetCurrentID(), newName);
            bool success = await UserDAO.Instance.update(updateUser);
            if (success)
            {
                CommonAnimations.HidePanel(changeNamePanel);
                changeNamePanel.SetActive(false);
            }
        }
    }
}
