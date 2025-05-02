using System;
using System.Collections;
using Assets.Scripts.Core.FireB;
using Assets.Scripts.Core.Models;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Init
{
    public class Authenticator : MonoBehaviour
    {

        [SerializeField] private GameObject namePanel;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private Button confirmButton;

        private bool signedInUnityService = false;
        private bool signedInFirebase = false;

        private void Awake()
        {
            InitEvents.OnUnityServicesSignIn += UnityServicesSignIn;
            InitEvents.OnFirebaseSignIn += FirebaseSignIn;
            startInit();
            confirmButton.onClick.AddListener(OnConfirmName);
        }

        private async void startInit()
        {
            await FirebaseActions.Init();
            await UnityServicesInit.Init();
        }

        private void OnDestroy()
        {
            InitEvents.OnUnityServicesSignIn -= UnityServicesSignIn;
            InitEvents.OnFirebaseSignIn -= FirebaseSignIn;
        }

        private void FirebaseSignIn()
        {
            Debug.Log("Firebase usuario autenticado, procediendo...");
            signedInFirebase = true;
        }

        private void UnityServicesSignIn()
        {
            signedInUnityService = true;
        }

        private void Update()
        {
            Debug.Log("Unity Services => " + signedInUnityService + " Firebase => " + signedInFirebase);
            if (signedInUnityService && signedInFirebase)
            {
                signedInUnityService = false;
                signedInFirebase = false;
                StartCoroutine(DelayedCheckRegistry());
            }
        }

        private IEnumerator DelayedCheckRegistry()
        {
            yield return null; 
            checkRegistry();
        }


        private async void checkRegistry()
        {
            string firebaseId = FirebaseActions.GetCurrentID();
            string unityId = UnityServicesInit.GetCurrentID();
            if (await UserDAO.Instance.exists(firebaseId, unityId))
            {
                await SceneManager.LoadSceneAsync("MenuScene");
            }
            else
            {
                namePanel.SetActive(true);
            }
        }

        private async void OnConfirmName()
        {
            string username = nameInput.text.Trim();

            if (!string.IsNullOrEmpty(username) && username.Length >= 3 && username.Length <= 20)
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
        }
    }
}
