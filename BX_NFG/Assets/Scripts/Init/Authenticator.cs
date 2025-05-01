using Assets.Scripts.Core.FireB;
using Assets.Scripts.Core.Models;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Init
{
    public class Authenticator : MonoBehaviour
    {

        [SerializeField] private GameObject namePanel;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private Button confirmButton;

        private void Awake()
        {
            confirmButton.onClick.AddListener(OnConfirmName);
        }

        async void Start()
        {
            bool unityServices = await UnityServicesInit.Init();
            bool firebaseServices = await FirebaseActions.Init();
            if ( unityServices && firebaseServices )
            {
                string firebaseId = FirebaseActions.GetCurrentID();
                string unityId = UnityServicesInit.GetCurrentID();
                if(await UserDAO.Instance.exists(firebaseId, unityId))
                {
                    await SceneManager.LoadSceneAsync("MenuScene");
                }else
                {
                    namePanel.SetActive(true);
                }
            }
        }

        private async void OnConfirmName()
        {
            string username = nameInput.text.Trim();

            if (!string.IsNullOrEmpty(username))
            {
                string firebaseId = FirebaseActions.GetCurrentID();
                string unityId = UnityServicesInit.GetCurrentID();

                var newUser = new User(firebaseId, unityId, username);
                bool success = await UserDAO.Instance.insert(newUser);

                if (success)
                {
                    await SceneManager.LoadSceneAsync("MenuScene");
                }
            }
            else
            {
                Debug.LogWarning("El nombre no puede estar vacío.");
            }
        }
    }
}
