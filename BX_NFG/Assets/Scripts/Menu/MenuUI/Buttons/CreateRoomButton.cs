
using Assets.Scripts.Commons;
using Assets.Scripts.Connection.Lobbi;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu.MenuUI.Buttons
{
    public class CreateRoomButton : BaseButton
    {
        protected async override void action()
        {
            bool success = await GameLobbyManager.Instance.CreateLobby();
            if (success) SceneManager.LoadSceneAsync(Scenes.Lobby.ToString());
        }
    }
}
