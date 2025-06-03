using UnityEngine;

namespace Assets.Scripts.UI.MenuUI.MenuActions
{
    public class MatchScroll : MonoBehaviour
    {
        [SerializeField] private RectTransform matchesScroll;
        [SerializeField] private GameObject matchPanelPrefab;

        private void Start()
        {
            SetMatchePanels();
        }

        /*
        private async Task<List<Match>> GetAllLocalMatches()
        {
            
        }
        */

        private async void SetMatchePanels()
        {
            
        }
    }
}
