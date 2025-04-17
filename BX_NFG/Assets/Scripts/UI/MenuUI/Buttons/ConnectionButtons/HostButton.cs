using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Buttons.ConnectionButtons
{
    public class HostButton : ButtonBase
    {
        public override void OnClick()
        {
            //NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }

        private void OnClientConnected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
                NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            }
        }
    }
}
