using Assets.Scripts.Input;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private readonly float moveSpeed = 10f;
    [SerializeField] private PlayerInput playerInput;
    private Rigidbody rb;


    private void Start()
    {
        playerInput.enabled = false;
        rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (IsHost) UpdateInputHost(playerInput.GetPlayerInput());
        if (IsOwner) UpdateInputServerRpc(playerInput.GetPlayerInput());
    }

    [Rpc(SendTo.Server)]
    private void UpdateInputServerRpc(Vector3 direction)
    {
        Vector3 newDirection = new Vector3(direction.x, 0, direction.y);
        Vector3 force = newDirection * moveSpeed;
        rb.linearVelocity = force;
    }
    private void UpdateInputHost(Vector3 direction)
    {
        Vector3 newDirection = new Vector3(direction.x, 0, direction.y);
        Vector3 force = newDirection * moveSpeed;
        rb.linearVelocity = force;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            playerInput.enabled = true;
        }
    }
}