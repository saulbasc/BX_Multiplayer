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
        [SerializeField] private TextMeshProUGUI _matchTimeText;
        [SerializeField] private Button _upperTimeButton;
        [SerializeField] private Button _lowerTimeButton;

        private void Awake()
        {
            if (!LobbyDataManager.Instance.IsLocalPlayerHost())
            {
                _upperTimeButton.gameObject.SetActive(false);
                _lowerTimeButton.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            _upperTimeButton.onClick.AddListener( async () => await LobbyActionsManager.Instance.ChangeMatchDuration(true));
            _lowerTimeButton.onClick.AddListener(async () => await LobbyActionsManager.Instance.ChangeMatchDuration(false));
            LobbyEvents.Instance.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            _upperTimeButton.onClick.AddListener(async () => await LobbyActionsManager.Instance.ChangeMatchDuration(true));
            _lowerTimeButton.onClick.RemoveListener(async () => await LobbyActionsManager.Instance.ChangeMatchDuration(false));
            LobbyEvents.Instance.OnLobbyUpdated -= OnLobbyUpdated;
        }

        private void OnLobbyUpdated()
        {
            MatchDuration newMatchDuration = LobbyDataManager.Instance.GetLobbyMatchDuration();
            _matchTimeText.text = MatchDurationExtensions.ToString(newMatchDuration);
        }
    }
}
