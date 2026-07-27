using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MenuUI.Components
{
    public class MatchPanel : MonoBehaviour
    {
        [SerializeField] private Image resultPanelImage;
        [SerializeField] private TextMeshProUGUI result;
        [SerializeField] private TextMeshProUGUI localScore;
        [SerializeField] private TextMeshProUGUI visitorScore;

        public void Initialize(MatchResult result, int localScore, int visitorScore)
        {
            switch (result)
            {
                case MatchResult.Draw:
                    this.result.text = "E";
                    resultPanelImage.color = Color.yellow;
                    break;
                case MatchResult.Win:
                    this.result.text = "V";
                    resultPanelImage.color = Color.green;
                    break;
                case MatchResult.Lose:
                    this.result.text = "L";
                    resultPanelImage.color = Color.red;
                    break;
            }

            this.localScore.text = localScore.ToString();
            this.visitorScore.text = visitorScore.ToString();
        }
    }

    public enum MatchResult
    {
        Draw = 0,
        Win = 1,
        Lose = 2
    }
}
