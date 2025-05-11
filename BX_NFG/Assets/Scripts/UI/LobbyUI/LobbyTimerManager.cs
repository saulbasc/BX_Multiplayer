using System;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Assets.Scripts.Handlers;
using Assets.Scripts.Lobbi.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Lobbi.UI.Config
{
    public class LobbyTimerManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI matchTimeText;
        [SerializeField] private Button upperTimeButton;
        [SerializeField] private Button lowerTimeButton;

        private void Awake()
        {
            inicializeMatchTimerButtons();
        }

        private void inicializeMatchTimerButtons()
        {
            if (!LobbyDataManager.Instance.IsHost())
            {
                upperTimeButton.gameObject.SetActive(false);
                lowerTimeButton.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            upperTimeButton.onClick.AddListener(() => IncreaseMatchTime());
            lowerTimeButton.onClick.AddListener(() => DecreaseMatchTime());
            GameLobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            upperTimeButton.onClick.RemoveListener(() => IncreaseMatchTime());
            lowerTimeButton.onClick.RemoveListener(() => DecreaseMatchTime());
            GameLobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        }

        private void OnLobbyUpdated()
        {
            MatchDuration newMatchDuration = LobbyDataManager.Instance.GetLobbyMatchDuration();
            matchTimeText.text = MatchDurationExtensions.ToString(newMatchDuration);
        }

        private async void IncreaseMatchTime()
        {
            MatchDuration[] durations = MatchDurationExtensions.MatchDurationList();
            MatchDuration currentMatchDuration = LobbyDataManager.Instance.GetLobbyMatchDuration();
            MatchDuration newMatchDuration = currentMatchDuration;

            int index = Array.IndexOf(durations, currentMatchDuration);
            if (index < durations.Length - 1)
            {
                newMatchDuration = durations[index + 1];
                await SafeAsyncFunctionsHandler.ExecuteAsync( async () =>
                {
                    await LobbyDataManager.Instance.SetLobbyMatchDurationAsync(newMatchDuration);
                });
            }
        }

        private async void DecreaseMatchTime()
        {
            MatchDuration[] durations = MatchDurationExtensions.MatchDurationList();
            MatchDuration currentMatchDuration = LobbyDataManager.Instance.GetLobbyMatchDuration();
            MatchDuration newMatchDuration = currentMatchDuration;

            int index = Array.IndexOf(durations, currentMatchDuration);
            if (index > 0)
            {
                newMatchDuration = durations[index - 1];
                await SafeAsyncFunctionsHandler.ExecuteAsync( async () =>
                {
                    await LobbyDataManager.Instance.SetLobbyMatchDurationAsync(newMatchDuration);
                });
            }
        }
    }
}
