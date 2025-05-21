using Assets.Scripts.Input;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private readonly float moveSpeed = 10f;
    [SerializeField] private PlayerInput playerInput;
    private Rigidbody rb;

    private Vector3 latestInput;

    private void Start()
    {
        playerInput.enabled = false;
        rb = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            playerInput.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            Vector3 input = playerInput.GetPlayerInput();

            if (IsHost)
            {
                latestInput = input;
            }
            else
            {
                UpdateInputServerRpc(input);
            }
        }

        if (IsServer)
        {
            Vector3 moveDirection = new Vector3(latestInput.x, 0, latestInput.y);
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }


    [Rpc(SendTo.Server)]
    private void UpdateInputServerRpc(Vector3 input)
    {
        latestInput = input;
    }
}
