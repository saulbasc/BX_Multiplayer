using Assets.Scripts.Game.GameEvents.Player;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

public class BallController : NetworkBehaviour
{
    private MatchStateManager matchStateManager;
    private Rigidbody ballRb;
    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;

    private NetworkVariable<ulong> LastPlayerTouched = new();

    public void SetLastTouched(ulong playerGameId)
    {
        LastPlayerTouched.Value = playerGameId;
    }

    public override void OnNetworkSpawn()
    {
        ballRb = GetComponent<Rigidbody>();

        if (IsServer)
        {
            GameObject manager = GameObject.Find("GameManager");
            matchStateManager = manager.GetComponent<MatchStateManager>();
            matchStateManager.OnMatchStateChanged += HandleStateChanged;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            matchStateManager.OnMatchStateChanged -= HandleStateChanged;
        }
    }

    private void HandleStateChanged(MatchState state)
    {
        if(state == MatchState.onGoal)
        {
            RegisterGoal();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsServer) return;

        var player = collision.gameObject.GetComponent<PlayerInGame>();
        if (player != null)
        {
            LastPlayerTouched.Value = player.OwnerClientId;
            player.RegisterTouch();
            Debug.Log($"Ball touched by player: {player.OwnerClientId}");
        }
    }

    private void RegisterGoal()
    {
        if (!IsServer) return;

        ulong lastTouchedPlayerId = LastPlayerTouched.Value;

        PlayerInGame[] players = FindObjectsByType<PlayerInGame>(FindObjectsSortMode.None);

        foreach (var player in players)
        {
            if (player.PlayerConnectionID == lastTouchedPlayerId)
            {
                player.AddGoal();
                Debug.Log($"Goal registered for player: {player.PlayerId} with ConnectionID: {lastTouchedPlayerId}");
                return;
            }
        }

        Debug.LogWarning($"No player found with ConnectionID: {lastTouchedPlayerId} to register goal.");
    }


    public void PauseBall()
    {
        if(!IsServer || ballRb == null) return;

        savedVelocity = ballRb.linearVelocity;
        savedAngularVelocity = ballRb.angularVelocity;
        ballRb.linearVelocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
    }

    public void ResumeBall()
    {
        if (!IsServer || ballRb == null) return;

        ballRb.linearVelocity = savedVelocity;
        ballRb.angularVelocity = savedAngularVelocity;
    }

    public void ShootBall(float shootForce, Vector3 playerPosition, ulong playerGameId)
    {
        Debug.Log("Shooting in ball Controller");
        LastPlayerTouched.Value = playerGameId;
        Vector3 direction = (ballRb.position - playerPosition).normalized;
        ballRb.AddForce(direction * shootForce, ForceMode.Impulse);
    }
}
