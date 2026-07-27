using Assets.Scripts.Core.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI.Components
{
    public class RankingStatPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerPosition;
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI playerStat;
        [SerializeField] private Image statImage;

        public void Initialize(int position, RankingStat rankingStat, Sprite image)
        {
            playerPosition.text = position.ToString();
            playerName.text = rankingStat.Name;
            playerStat.text = rankingStat.Value.ToString();
            if (statImage != null)
            {
                statImage.sprite = image;
            }
        }
    }
}
