using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Buttons.ConnectionButtons
{
    public class HostButton : ButtonBase
    {
        public override void OnClick()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.StartHost();
        }

        private void OnClientConnected(ulong clientId)
        {
            // Solo el host debe cargar la escena
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
                NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            }
        }
    }
}
