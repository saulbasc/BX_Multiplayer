using Assets.Scripts.Game.Manager;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;

public class BallController : NetworkBehaviour
{
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
            MatchStateManager.Instance.OnMatchStateChanged += HandleStateChanged;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            MatchStateManager.Instance.OnMatchStateChanged -= HandleStateChanged;
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

        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            LastPlayerTouched.Value = player.OwnerClientId;
            Debug.Log($"Ball touched by player: {player.OwnerClientId}");
        }
    }

    private void RegisterGoal()
    {
        if (!IsServer) return;
        
        foreach(var (key, player) in MatchInfo.Instance.Match.LocalTeam.Players)
        {
            if (player.PlayerGameId == LastPlayerTouched.Value)
            {
                player.AddGoal();
                return;
            }
        }

        foreach (var (key, player) in MatchInfo.Instance.Match.VisitorTeam.Players)
        {
            if (player.PlayerGameId == LastPlayerTouched.Value)
            {
                player.AddGoal();
                return;
            }
        }
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
