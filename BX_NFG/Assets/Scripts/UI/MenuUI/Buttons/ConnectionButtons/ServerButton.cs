using Unity.Netcode;

namespace Assets.Scripts.UI.Buttons.ConnectionButtons
{
    public class ServerButton : ButtonBase
    {
        public override void OnClick()
        {
            NetworkManager.Singleton.StartServer();
        }
    }
}
