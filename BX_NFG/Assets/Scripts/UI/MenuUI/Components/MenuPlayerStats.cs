using Assets.Scripts.Core.Daos;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Init;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.MenuUI.Components
{
    public class MenuPlayerStats : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerMatches;
        [SerializeField] private TextMeshProUGUI playerMinutes;
        [SerializeField] private TextMeshProUGUI playerGoals;
        [SerializeField] private TextMeshProUGUI playerTouches;

        private void Start()
        {
            SetPlayerStats();
        }

        private async void SetPlayerStats()
        {
            PlayerStats stats = await PlayerStatsDAO.Instance.Select(UnityServicesActions.GetCurrentUserID());
            if (stats != null)
            {
                playerMatches.text = stats.MatchesPlayed.ToString();
                int minutesPlayed = (int)stats.SecondsPlayed / 60;
                playerMinutes.text = minutesPlayed.ToString();
                playerGoals.text = stats.Goals.ToString();
                playerTouches.text = stats.Touches.ToString();
            }
            else
            {
                Debug.LogWarning("Player stats not found for the current user.");
            }
        }
    }
}
