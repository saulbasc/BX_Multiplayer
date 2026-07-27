using System.Collections.Generic;
using Assets.Scripts.Core.Daos;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Init;
using Assets.Scripts.UI.MenuUI.Components;
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

        private async void SetMatchePanels()
        {
            List<PlayerMatchSummary> summaries = await PlayerMatchSummaryDAO.Instance.SelectAll(UnityServicesActions.GetCurrentUserID());

            foreach (PlayerMatchSummary summary in summaries)
            {
                GameObject matchPanel = Instantiate(matchPanelPrefab, matchesScroll);
                MatchPanel panel = matchPanel.GetComponent<MatchPanel>();
                panel.Initialize(summary.Result, summary.LocalScore, summary.VisitorScore);
            }
        }
    }
}
