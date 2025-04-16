using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceMenuInEditor : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private string menuSceneName = "MenuScene";
    private bool loaded = false;

    private void Awake()
    {
        if(loaded) return;
        if (SceneManager.GetActiveScene().name != menuSceneName)
        {
            loaded = true;
            SceneManager.LoadScene(menuSceneName);
        }
    }
#endif
}

