using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Connection.Lobbi;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Assets.Scripts.Lobbi.Datas;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Lobbi.UI.Config
{
    public class MatchTimerConfig : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI matchTimeText;
        [SerializeField] private Button upperTimeButton;
        [SerializeField] private Button lowerTimeButton;

        private void Awake()
        {
            if (!GameLobbyManager.Instance.IsHost())
            {
                upperTimeButton.gameObject.SetActive(false);
                lowerTimeButton.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            upperTimeButton.onClick.AddListener(() => IncreaseMatchTime());
            lowerTimeButton.onClick.AddListener(() => DecreaseMatchTime());
        }
        private void OnDisable()
        {
            upperTimeButton.onClick.RemoveListener(() => IncreaseMatchTime());
            lowerTimeButton.onClick.RemoveListener(() => DecreaseMatchTime());
        }

        private void DecreaseMatchTime()
        {
            MatchDuration newMatchDuration = decrease();
            matchTimeText.text = newMatchDuration.ToString();
        }

        private void IncreaseMatchTime()
        {
            Task<MatchDuration> newMatchDuration = increase();
            matchTimeText.text = newMatchDuration.ToString();
        }

        private async Task<MatchDuration> increase()
        {
            MatchDuration[] durations = new MatchDuration[]
            {
                MatchDuration.matchDuration1,
                MatchDuration.matchDuration3,
                MatchDuration.matchDuration5,
                MatchDuration.matchDuration7,
                MatchDuration.matchDuration10
            };

            var currentLobbyData = LobbyManager.Instance.GetLobbyData();
            var newLobbyData = new LobbyData(currentLobbyData);
            MatchDuration currentMatchDuration = newLobbyData.MatchDuration; 

            MatchDuration newMatchDuration = currentMatchDuration;

            int index = Array.IndexOf(durations, currentMatchDuration);
            if (index < durations.Length - 1)
            {
                newMatchDuration = durations[index + 1];
                newLobbyData.MatchDuration = newMatchDuration;
                await LobbyManager.Instance.UpdateLobbyData(newLobbyData.Serialize());
            }
            return newMatchDuration;
        }

        private MatchDuration decrease()
        {
            throw new NotImplementedException();
        }
    }
}
