using Unity.Netcode;

namespace Assets.Scripts.UI.Buttons.ConnectionButtons
{
    public class HostButton : ButtonBase
    {
        public override void OnClick()
        {
            NetworkManager.Singleton.StartHost();
        }
    }
}
