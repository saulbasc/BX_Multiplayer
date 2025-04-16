using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Buttons.ConnectionButtons
{
    public class ServerButton : ButtonBase
    {
        public override void OnClick()
        {
            NetworkManager.Singleton.StartServer();
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }
}
