
using System.Threading.Tasks;
using Assets.Scripts.Core.FireB;
using Assets.Scripts.Core.Models;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI profileButtonText;
        [SerializeField] private GameObject loadingPanel;

        private void Awake()
        {
            
        }

        private async void Start()
        {
            loadingPanel.SetActive(true);
            await setNamePlayer();
            loadingPanel.SetActive(false);
        }

        private async Task setNamePlayer()
        {
            User user = await UserDAO.Instance.select(FirebaseActions.GetCurrentID());
            if (user != null)
            {
                profileButtonText.text = user.Username;
            }
        }
    }
}
