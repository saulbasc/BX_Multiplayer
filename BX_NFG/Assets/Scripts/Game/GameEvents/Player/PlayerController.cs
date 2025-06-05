using System;
using System.Collections;
using Assets.Scripts.GameManager.GameEvents;
using Assets.Scripts.GameManager.GameEvents.State;
using Assets.Scripts.Input;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private MatchStateManager matchStateManager;
    private readonly float moveSpeed = 18f;

    [SerializeField] private GameJoystick playerInput;
    private Rigidbody playerRb;
    private Vector3 latestInput;

    private bool updateable;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerInput.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            playerInput.enabled = true;
        }

        if (IsServer)
        {
            StartCoroutine(WaitForGameManager());
            MatchSpawnerManager.OnTeleportingChanged += OnTeleportingChanged;
            updateable = false;
        }
    }

    private IEnumerator WaitForGameManager()
    {
        GameObject manager = null;
        while (manager == null)
        {
            manager = GameObject.Find("GameManager");
            yield return null; 
        }

        matchStateManager = manager.GetComponent<MatchStateManager>();

        if (matchStateManager != null)
        {
            matchStateManager.OnMatchStateChanged += HandleStateChanged;
        }
        else
        {
            Debug.LogError("MatchStateManager component not found on GameManager!");
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            if (matchStateManager != null)
            {
                matchStateManager.OnMatchStateChanged -= HandleStateChanged;
            }
            MatchSpawnerManager.OnTeleportingChanged -= OnTeleportingChanged;
        }
    }


    private void HandleStateChanged(MatchState state)
    {
        if (state == MatchState.pause || state == MatchState.gameOver || state == MatchState.preMatch)
        {
            updateable = false;
        } 
        else
        {
            updateable = true;
        }
    }

    private void OnTeleportingChanged(bool isTeleporting)
    {
        updateable = !isTeleporting;

        if (!isTeleporting)
        {
            StartCoroutine(ReenableMovement());
        }
    }

    private IEnumerator ReenableMovement()
    {
        yield return new WaitForSeconds(0.1f);
        updateable = true;
    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            Vector3 input = playerInput.GetPlayerInput();

            if (IsServer)
            {
                latestInput = input; 
            }
            else
            {
                UpdateInputServerRpc(input);
            }
        }

        if (IsServer && updateable)
        {
            Vector3 moveDirection = new Vector3(latestInput.x, 0, latestInput.y);
            playerRb.linearVelocity = moveDirection * moveSpeed;
        }
    }

    [Rpc(SendTo.Server)]
    private void UpdateInputServerRpc(Vector3 input)
    {
        latestInput = input;
    }
}
