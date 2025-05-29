using Assets.Scripts.Input;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private readonly float moveSpeed = 10f;

    [SerializeField] private GameJoystick playerInput;
    private Rigidbody rb;

    // Último input procesado por el servidor
    private Vector3 latestInput;

    // Posición sincronizada desde el servidor
    private NetworkVariable<Vector3> serverPosition = new(writePerm: NetworkVariableWritePermission.Server);

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput.enabled = false;
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
            Vector3 moveDirection = new Vector3(input.x, 0, input.y);
            rb.linearVelocity = moveDirection * moveSpeed;

            if (!IsHost)
            {
                UpdateInputServerRpc(input);
            }

            latestInput = input; // guardar localmente para reconciliación si es necesario
        }

        if (IsServer)
        {
            Vector3 moveDirection = new Vector3(latestInput.x, 0, latestInput.y);
            rb.linearVelocity = moveDirection * moveSpeed;
            serverPosition.Value = transform.position;
        }
    }

    private void Update()
    {
        if (IsOwner && !IsServer)
        {
            // Reconciliación suave con la posición del servidor
            float dist = Vector3.Distance(transform.position, serverPosition.Value);
            if (dist > 0.5f)
            {
                transform.position = Vector3.Lerp(transform.position, serverPosition.Value, 0.25f);
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void UpdateInputServerRpc(Vector3 input)
    {
        latestInput = input;
    }
}
