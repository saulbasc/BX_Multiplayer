
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Logic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.UI.GameUI
{
    public class GamePlayerInfo : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameTag;
        public override void OnNetworkSpawn()
        {
            var data = LobbyPlayerManager.Instance.GetSinglePlayerDataObject(UnityServicesActions.GetCurrentUserID());
            playerNameTag.text = data.GameTag;
        }
    }
}
