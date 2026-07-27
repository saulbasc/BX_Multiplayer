using Assets.Scripts.Core.Daos;
using Assets.Scripts.Core.Models;
using Assets.Scripts.UI.MenuUI.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.MenuUI.MenuActions
{
    public class RankingManager : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown rankingDropdown;
        [SerializeField] private RectTransform rankingScroll;
        [SerializeField] private GameObject rankingPrefab;

        [SerializeField] private List<Sprite> rankingImages;

        private void Start()
        {
            rankingDropdown.onValueChanged.AddListener(async (index) => await OnRankingTypeChanged(index));
            FirstSearch();
        }

        private async void FirstSearch()
        {
            await OnRankingTypeChanged(rankingDropdown.value);
        }

        private async Task OnRankingTypeChanged(int index)
        {
            ClearScroll();

            RankingType selectedType = (RankingType)index;

            List<RankingStat> rankingStats = await RankingDAO.Instance.GetRanking(selectedType, 10);
            int position = 1;

            foreach (var stat in rankingStats)
            {
                GameObject prefab = Instantiate(rankingPrefab, rankingScroll);
                prefab.GetComponent<RankingStatPanel>().Initialize(position, stat, rankingImages[index]);
                position++;
            }
        }

        private void ClearScroll()
        {
            foreach (Transform child in rankingScroll)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
