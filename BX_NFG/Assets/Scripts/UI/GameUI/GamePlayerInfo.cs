using Assets.Scripts.Game.GameEvents.Player;
using Assets.Scripts.Lobbi.Data;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayerInfo : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameTag;
    [SerializeField] private Image teamSpriteRenderer; 
    [SerializeField] private Sprite localSprite;
    [SerializeField] private Sprite visitorSprite;

    private NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>(
        "", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private NetworkVariable<bool> isLocalTeam = new NetworkVariable<bool>(
        false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private PlayerInGame playerInGame;
    private bool setPlayerInfo = false;

    public override void OnNetworkSpawn()
    {
        playerName.OnValueChanged += OnPlayerNameChanged;
        isLocalTeam.OnValueChanged += OnTeamChanged;

        if (IsServer)
        {
            playerInGame = GetComponent<PlayerInGame>();

            if (!string.IsNullOrEmpty(playerInGame.TagName))
            {
                playerName.Value = playerInGame.TagName;
                isLocalTeam.Value = playerInGame.Team == PlayerTeam.Local;
                setPlayerInfo = true;
            }
        }
        else
        {
            playerNameTag.text = playerName.Value.ToString();
            SetTeamVisual(isLocalTeam.Value);
        }
    }

    public override void OnNetworkDespawn()
    {
        playerName.OnValueChanged -= OnPlayerNameChanged;
        isLocalTeam.OnValueChanged -= OnTeamChanged;
    }

    private void Update()
    {
        if (IsServer && !setPlayerInfo)
        {
            if (!string.IsNullOrEmpty(playerInGame?.TagName))
            {
                playerName.Value = playerInGame.TagName;
                isLocalTeam.Value = playerInGame.Team == PlayerTeam.Local;
                setPlayerInfo = true;
            }
        }
    }

    private void OnPlayerNameChanged(FixedString32Bytes oldValue, FixedString32Bytes newValue)
    {
        playerNameTag.text = newValue.ToString();
    }

    private void OnTeamChanged(bool oldValue, bool newValue)
    {
        SetTeamVisual(newValue);
    }

    private void SetTeamVisual(bool isLocal)
    {
        if (teamSpriteRenderer == null) return;

        Debug.Log("ES LOCAL?????? => " + isLocal);
        teamSpriteRenderer.sprite = isLocal ? localSprite : visitorSprite;
    }
}
