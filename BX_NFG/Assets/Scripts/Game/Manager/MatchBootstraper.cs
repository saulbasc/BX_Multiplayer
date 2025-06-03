using Unity.Netcode;
using UnityEngine;

public class MatchBootstraper : MonoBehaviour
{
    [SerializeField] private GameObject matchStateManagerPrefab;

    private void Start()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
        {
            var instance = Instantiate(matchStateManagerPrefab);
            instance.GetComponent<NetworkObject>().Spawn(true);
            Debug.Log("[SERVER] MatchStateManager instanciado y spawneado.");
        }
    }
}
