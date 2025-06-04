using Assets.Scripts.Game.Manager;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.UI.GameUI.Components;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.GameUI.GamePanels
{
    public class GameOverPanelStats : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI localScore;
        [SerializeField] private TextMeshProUGUI visitorScore;
        [SerializeField] private RectTransform localScroll;
        [SerializeField] private RectTransform visitorScroll;
        [SerializeField] private GameObject statsPrefab;

        private void OnEnable()
        {
            SetScore();
            SetPanels();
        }

        private void SetScore()
        {
            localScore.text = MatchInfo.Instance.GetLocalScore().ToString();
            visitorScore.text = MatchInfo.Instance.GetVisitorScore().ToString();
        }

        private void SetPanels()
        {
            foreach (var player in MatchInfo.Instance.GetPlayersInGame())
            {
                GameObject stats; 
                if (player.Team == PlayerTeam.Local)
                {
                    stats = Instantiate(statsPrefab, localScroll);
                }
                else if (player.Team == PlayerTeam.Visitor)
                {
                    stats = Instantiate(statsPrefab, visitorScroll);
                }
                else
                {
                    return;
                }

                var playerStats = stats.GetComponent<PlayerGameOverStats>();
                if (playerStats != null)
                {
                    playerStats.SetPanelData(player.TagName, player.Goals, player.Touches);
                }
                else
                {
                    Debug.LogError("PlayerGameOverStats component not found on the instantiated prefab.");
                }
            }
        }
    }
}
