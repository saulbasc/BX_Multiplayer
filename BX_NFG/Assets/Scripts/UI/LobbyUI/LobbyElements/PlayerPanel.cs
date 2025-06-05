using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Logic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Init;
using System.Collections;

namespace Assets.Scripts.Lobbi.UI.PlayerEntry
{
    public class PlayerPanel : MonoBehaviour
    {
        private LobbyPlayerManager lobbyPlayerManager;

        [System.Serializable]
        public struct TeamButtonBinding
        {
            public Button button;
            public PlayerTeam team;
        }

        [SerializeField] private List<TeamButtonBinding> teamButtons;

        private void OnEnable()
        {
            StartCoroutine(FindLobbyPlayerManager());
        }

        private IEnumerator FindLobbyPlayerManager()
        {
            while (lobbyPlayerManager == null)
            {
                lobbyPlayerManager = FindFirstObjectByType<LobbyPlayerManager>();
                if (lobbyPlayerManager == null)
                {
                    yield return null; 
                }
            }

            foreach (var binding in teamButtons)
            {
                binding.button.onClick.AddListener(() => OnButtonClicked(binding.team));
            }
        }

        private void OnDisable()
        {
            foreach (var binding in teamButtons)
            {
                binding.button.onClick.RemoveAllListeners();
            }
        }

        private async void OnButtonClicked(PlayerTeam team)
        {
            if (lobbyPlayerManager == null) return;

            string localId = UnityServicesActions.GetCurrentUserID();
            await lobbyPlayerManager.SetPlayerTeamAsync(
                lobbyPlayerManager.GetSinglePlayerDataObject(localId), 
                team
            );
        }
    }
}