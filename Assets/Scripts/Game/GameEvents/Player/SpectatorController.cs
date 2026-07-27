using Unity.Netcode;
using UnityEngine;

public class SpectatorController : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Debug.Log("🎥 Soy el espectador local.");
        }
        else
        {
            Debug.Log("👀 Otro espectador ha aparecido.");
        }
    }
}
