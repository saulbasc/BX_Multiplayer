using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.UI.MenuUI.Components
{
    public class MatchPanel : NetworkBehaviour
    {
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TextMeshProUGUI result;
        [SerializeField] private TextMeshProUGUI localScore;
        [SerializeField] private TextMeshProUGUI visitorScore;

        public void Initialize(MatchResult result, int localScore, int visitorScore)
        {
            switch (result)
            {
                case MatchResult.Draw:
                    this.result.text = "E";
                    this.result.color = Color.yellow;
                    break;
                case MatchResult.Win:
                    this.result.text = "V";
                    this.result.color = Color.green;
                    break;
                case MatchResult.Lose:
                    this.result.text = "L";
                    this.result.color = Color.red;
                    break;
            }
        }
    }

    public enum MatchResult
    {
        Draw = 0,
        Win = 1,
        Lose = 2
    }
}
