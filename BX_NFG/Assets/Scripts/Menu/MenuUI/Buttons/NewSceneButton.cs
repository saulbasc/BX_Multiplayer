using Assets.Scripts.Commons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu.MenuUI.Buttons
{
    public class NewSceneButton : BaseButton
    {
        [SerializeField] private Scenes scene;

        protected override void action()
        {
            SceneManager.LoadScene(scene.ToString());
        }
    }
}
