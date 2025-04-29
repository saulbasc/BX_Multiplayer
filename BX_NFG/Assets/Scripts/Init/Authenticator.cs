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
            bool firebaseServices = await FirebaseInit.Init();
            if ( unityServices && firebaseServices )
            {
                UserDAO userDao = new UserDAO();
                string firebaseId = FirebaseInit.GetCurrentID();
                string unityId = UnityServicesInit.GetCurrentID();
                if(await userDao.exists(firebaseId, unityId))
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
                UserDAO userDAO = new UserDAO();

                string firebaseId = FirebaseInit.GetCurrentID();
                string unityId = UnityServicesInit.GetCurrentID();

                var newUser = new User(firebaseId, unityId, username);
                bool success = await userDAO.insert(newUser);

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
