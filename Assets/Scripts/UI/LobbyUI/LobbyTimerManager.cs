using System.Collections;
using Assets.Scripts.GameManager.GameEvents.Timer;
using Assets.Scripts.Lobbi.Logic;
using Assets.Scripts.UI.LobbyUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Lobbi.UI.Config
{
    /// <summary>
    /// Clase dedicada a gestionar los cambios en la duración del partido.
    /// </summary>
    public class LobbyTimerManager : MonoBehaviour
    {
        private LobbyDataManager lobbyDataManager;
        [SerializeField] private TextMeshProUGUI _matchTimeText;
        [SerializeField] private Button _upperTimeButton;
        [SerializeField] private Button _lowerTimeButton;

        private bool canStart;

        [SerializeField] private LobbyActionsManager lobbyActionsManager;

        private void OnEnable()
        {
            LobbyEvents.Instance.OnLobbyReadyToStart += () => canStart = true;
            StartCoroutine(SetLobbyTimer());
        }

        private IEnumerator SetLobbyTimer()
        {
            while (lobbyDataManager == null || lobbyActionsManager == null)
            {
                lobbyDataManager = FindFirstObjectByType<LobbyDataManager>();
                lobbyActionsManager = FindFirstObjectByType<LobbyActionsManager>();
                yield return null;
            }

            while (!canStart)
            {
                yield return null;
            }

            if (!lobbyDataManager.IsLocalPlayerHost())
            {
                _upperTimeButton.gameObject.SetActive(false);
                _lowerTimeButton.gameObject.SetActive(false);
            }

            _upperTimeButton.onClick.AddListener(IncreaseMatchDuration);
            _lowerTimeButton.onClick.AddListener(DecreaseMatchDuration);
            LobbyEvents.Instance.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            _upperTimeButton.onClick.RemoveListener(IncreaseMatchDuration);
            _lowerTimeButton.onClick.RemoveListener(DecreaseMatchDuration);
            LobbyEvents.Instance.OnLobbyUpdated -= OnLobbyUpdated;
        }

        private async void IncreaseMatchDuration()
        {
            await lobbyActionsManager.ChangeMatchDuration(true);
        }

        private async void DecreaseMatchDuration()
        {
            await lobbyActionsManager.ChangeMatchDuration(false);
        }

        private void OnLobbyUpdated()
        {
            MatchDuration newMatchDuration = lobbyDataManager.GetLobbyMatchDuration();
            _matchTimeText.text = MatchDurationExtensions.ToString(newMatchDuration);
        }
    }
}
