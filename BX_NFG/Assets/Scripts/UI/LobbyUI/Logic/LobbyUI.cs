using Assets.Scripts.Lobbi.Logic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.LobbyUI
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lobbyNameText;

        private void Start()
        {
            InicialiceLobbyUI();
        }

        private void InicialiceLobbyUI()
        {
            lobbyNameText.text = LobbyDataManager.Instance.GetLobbyCode();
        }
    }
}
