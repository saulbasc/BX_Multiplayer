using UnityEngine;
using Unity.Services.Core;
using System.Collections;

public class UnitySessionMonitor : MonoBehaviour
{
    private static UnitySessionMonitor instance;

    [SerializeField] private GameObject lostConnectionPanel;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(MonitorSessionRoutine());
    }

    private IEnumerator MonitorSessionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                lostConnectionPanel.SetActive(true);
                Debug.LogWarning("[UnitySessionMonitor] Unity Services han perdido la sesión.");
            }
            else
            {
                Debug.Log("[UnitySessionMonitor] Sesión activa.");
            }
        }
    }
}
