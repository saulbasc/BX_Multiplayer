using Assets.Scripts.GameManager.GameEvents.State;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionManager : NetworkBehaviour
{
    [SerializeField] MatchStateManager matchStateManager;
    private int matchState = 0;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        matchStateManager.OnMatchStateChanged += HandleStateChanged;
    }

    private void Update()
    {
        if ( matchState == 1 )
        {
            if(NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
                Shutdown();
            }
            else
            {
                SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
            }
        }
    }

    private void HandleStateChanged(MatchState state)
    {
        if(state == MatchState.gameOver)
        {
            matchState = 1;
        }
    }

    private void Shutdown()
    {
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
