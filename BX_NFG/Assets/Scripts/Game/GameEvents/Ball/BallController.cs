
using Unity.Netcode;
using UnityEngine;

public class BallController : NetworkBehaviour
{
    private Rigidbody rb;
    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;

    private NetworkVariable<ulong> LastPlayerTouched = new();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        rb = GetComponent<Rigidbody>();
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

    public void PauseBall()
    {
        savedVelocity = rb.linearVelocity;
        savedAngularVelocity = rb.angularVelocity;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void ResumeBall()
    {
        if (!IsServer || rb == null) return;

        rb.linearVelocity = savedVelocity;
        rb.angularVelocity = savedAngularVelocity;
    }
}
