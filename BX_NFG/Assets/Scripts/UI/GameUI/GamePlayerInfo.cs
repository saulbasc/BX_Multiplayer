using Assets.Scripts.Game.GameEvents.Player;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class GamePlayerInfo : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameTag;

    private NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>(
        "", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private PlayerInGame playerInGame;
    private bool setPlayerInfo = false;

    public override void OnNetworkSpawn()
    {
        playerName.OnValueChanged += OnPlayerNameChanged;

        if (IsServer)
        {
            playerInGame = GetComponent<PlayerInGame>();
        }

        playerNameTag.text = playerName.Value.ToString();
    }

    public override void OnNetworkDespawn()
    {
        playerName.OnValueChanged -= OnPlayerNameChanged;
    }

    private void Update()
    {
        if (IsServer && !setPlayerInfo)
        {
            if (playerInGame.TagName != null)
            {
                playerName.Value = playerInGame.TagName;
                setPlayerInfo = true;
            }
        }
    }

    private void OnPlayerNameChanged(FixedString32Bytes oldValue, FixedString32Bytes newValue)
    {
        playerNameTag.text = newValue.ToString();
    }
}
