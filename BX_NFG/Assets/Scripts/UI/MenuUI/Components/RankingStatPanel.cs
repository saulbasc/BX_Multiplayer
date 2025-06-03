using Assets.Scripts.Core.Models;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.MenuUI.Components
{
    public class RankingStatPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI playerStat;

        public void Initialize(RankingStat rankingStat)
        {
            playerName.text = rankingStat.Name;
            playerStat.text = rankingStat.Value.ToString();
        }
    }
}
