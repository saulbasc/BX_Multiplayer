using System.Collections;
using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionManager : NetworkBehaviour
{
    [SerializeField] MatchStateManager matchStateManager;
    private int globalState = 0;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        matchStateManager.OnMatchStateChanged += HandleStateChanged;
    }

    private void HandleStateChanged(MatchState state)
    {
        if(state == MatchState.gameOver)
        {
            globalState = 1;
        }
    }

    private void Update()
    {
        if (globalState == 1)
        { 
            if (IsServer)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);

                StartCoroutine(DelayedShutdown());
            }
            else
            {
                SceneManager.LoadScene("MenuScene");
            }
        }
    }

    private IEnumerator DelayedShutdown()
    {
        yield return new WaitForSeconds(1f);

        DisconnectAllClients();
        DestroyRoom();
    }

    private void DisconnectAllClients()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            if (client.Key != NetworkManager.Singleton.LocalClientId)
            {
                NetworkManager.Singleton.DisconnectClient(client.Key);
            }
        }
    }

    private void DestroyRoom()
    {
        NetworkManager.Singleton.Shutdown();
    }
}
