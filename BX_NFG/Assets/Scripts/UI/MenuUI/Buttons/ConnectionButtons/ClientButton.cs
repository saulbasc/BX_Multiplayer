using Unity.Netcode;

namespace Assets.Scripts.UI.Buttons.ConnectionButtons
{
    public class ClientButton : ButtonBase
    {
        public override void OnClick()
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
