using Assets.Scripts.Game.GameEvents.Player;
using Assets.Scripts.Init;
using Assets.Scripts.Lobbi.Logic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class GamePlayerInfo : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameTag;

    private NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>(
        "", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        playerName.OnValueChanged += OnPlayerNameChanged;

        playerNameTag.text = playerName.Value.ToString();

        if (IsOwner)
        {
            PlayerInGame playerInGame = GetComponent<PlayerInGame>();
            SetPlayerNameServerRpc(new FixedString32Bytes(playerInGame.TagName));
        }
    }

    private void OnPlayerNameChanged(FixedString32Bytes oldValue, FixedString32Bytes newValue)
    {
        playerNameTag.text = newValue.ToString();
    }

    [ServerRpc]
    private void SetPlayerNameServerRpc(FixedString32Bytes name)
    {
        playerName.Value = name;
    }

    public override void OnNetworkDespawn()
    {
        playerName.OnValueChanged -= OnPlayerNameChanged;
    }
}
