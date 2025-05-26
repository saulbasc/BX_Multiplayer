
using System;
using Assets.Scripts.Core.FireB;
using Assets.Scripts.Core.Models;
using Assets.Scripts.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI
{
    public class ChangeProfileNamePanel : PanelBase
    {
        public override PanelType PanelType => PanelType.ChangeProfileNamePanel;

        [SerializeField] private Button backButton;
        [SerializeField] private Button confirmNameButton;
        [SerializeField] private TMP_InputField nameInput;

        public override void Initialize(IUIManager manager)
        {
            base.manager = manager;

            backButton.onClick.AddListener(() => base.manager.RemoveFloatPanel(PanelType));
            confirmNameButton.onClick.AddListener(OnConfirmButtonClicked);
        }

        private async void OnConfirmButtonClicked()
        {
            string newName = nameInput.text;
            User updatedUser = new User(FirebaseActions.GetCurrentID(), newName);
            await UserDAO.Instance.updates(updatedUser);
            manager.RemoveFloatPanel(PanelType);
        }
    }
}
