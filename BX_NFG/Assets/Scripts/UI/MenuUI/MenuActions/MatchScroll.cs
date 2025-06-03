using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Core.Daos;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Init;
using NUnit.Framework;
using Unity.Services.Core;
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

        private async Task<List<Match>> GetAllLocalMatches()
        {
            return await MatchDAO.Instance.SelectAllWithPlayerID(UnityServicesActions.GetCurrentUserID());
        }

        private async void SetMatchePanels()
        {
            
        }
    }
}
