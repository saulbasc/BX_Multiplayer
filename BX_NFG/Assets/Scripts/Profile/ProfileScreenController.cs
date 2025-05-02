using System;
using System.Threading.Tasks;
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
        [SerializeField] private GameObject loadingPanel;

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

        private async void OnConfirmNameButtonClicked()
        {
            await changeNamePlayer(inputField.text);
            await setNamePlayer();
        }

        private async void Start()
        {
            loadingPanel.SetActive(true);
            await setNamePlayer();
            loadingPanel.SetActive(false);
        }

        private async Task setNamePlayer()
        {
            User user = await UserDAO.Instance.select(FirebaseActions.GetCurrentID());
            if (user != null)
            {
                profileName.text = user.Username;
            }
        }

        private async Task changeNamePlayer(string newName)
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
