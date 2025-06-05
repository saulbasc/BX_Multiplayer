using System;
using System.Collections;
using Assets.Scripts.Game.Manager;
using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.UI.GameUI.Components;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.GameUI.GamePanels
{
    public class GameOverPanelStats : MonoBehaviour
    {
        private MatchInfo matchInfo;

        [SerializeField] private TextMeshProUGUI localScore;
        [SerializeField] private TextMeshProUGUI visitorScore;
        [SerializeField] private RectTransform localScroll;
        [SerializeField] private RectTransform visitorScroll;
        [SerializeField] private GameObject statsPrefab;

        private void OnEnable()
        {
            StartCoroutine(WaitForMatchInfo());
        }

        private IEnumerator WaitForMatchInfo()
        {
            GameObject manager = null;
            while (manager == null)
            {
                manager = GameObject.Find("GameManager");
                yield return null;
            }

            matchInfo = manager.GetComponent<MatchInfo>();

            yield return new WaitUntil(() => matchInfo.playerStats.Count > 0);

            SetScore();
            SetPanels();
        }

        private void SetScore()
        {
            if (matchInfo == null) return;

            localScore.text = matchInfo.GetLocalScore().ToString();
            visitorScore.text = matchInfo.GetVisitorScore().ToString();
        }

        private void SetPanels()
        {
            foreach (var player in matchInfo.playerStats)
            {
                GameObject stats; 
                if (player.PlayerTeam == PlayerTeam.Local)
                {
                    stats = Instantiate(statsPrefab, localScroll);
                }
                else if (player.PlayerTeam == PlayerTeam.Visitor)
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
                    playerStats.SetPanelData(player.PlayerName.ToString(), player.Goals, player.Touches);
                }
                else
                {
                    Debug.LogError("PlayerGameOverStats component not found on the instantiated prefab.");
                }
            }
        }
    }
}
