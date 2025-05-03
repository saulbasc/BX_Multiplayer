
using Assets.Scripts.Commons;
using Assets.Scripts.Connection.Lobbi;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu.MenuUI.Buttons
{
    public class CreateRoomButton : BaseButton
    {
        protected async override void action()
        {
            bool success = await GamePlayersManager.Instance.CreateLobby();
            if (success) SceneManager.LoadSceneAsync(Scenes.Lobby.ToString());
        }
    }
}
