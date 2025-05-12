using Assets.Scripts.Lobbi.Data;
using Assets.Scripts.Lobbi.Logic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Lobbi.Players;
using Assets.Scripts.Init;

namespace Assets.Scripts.Lobbi.UI.PlayerEntry
{
    public class PlayerPanel : MonoBehaviour
    {
        [System.Serializable]
        public struct TeamButtonBinding
        {
            public Button button;
            public PlayerTeam team;
        }

        [SerializeField] private List<TeamButtonBinding> teamButtons;

        private void OnEnable()
        {
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
            string localId = UnityServicesActions.GetCurrentUserID();
            await LobbyPlayerManager.Instance.SetPlayerTeamAsync(
                LobbyPlayerManager.Instance.GetSinglePlayerDataObject(localId), 
                team
            );
        }
    }
}