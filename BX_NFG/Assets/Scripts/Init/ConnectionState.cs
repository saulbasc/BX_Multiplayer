using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Corroutine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Commons;

/// <summary>
/// Clase que gestiona el estado de la conexión del usuario dentro del juego
/// </summary>
public class ConnectionState : MonoBehaviour
{
    private static ConnectionState instance;

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
        CoroutineManager.Instance.StartTrackedCoroutine(
            CoroutineIndentifier.ConnectionStateCorrutine, 
            ConnectionStateCoroutine()
        );
    }

    /// <summary>
    /// Corrutina que verifica periódicamente la conexión del dispositivo.
    /// Si se pierde la conexión volverá a la pantalla inicial
    /// </summary>
    /// <returns>Corrutina que se ejecuta indefinidamente cada 5 segundos.</returns>
    private IEnumerator ConnectionStateCoroutine()
    {
        bool wasConnected = true;

        while (true)
        {
            yield return new WaitForSeconds(5f);

            bool isConnected = Application.internetReachability != NetworkReachability.NotReachable;

            if (isConnected && !wasConnected) OnReconnect();
            else if (!isConnected && wasConnected) OnConnectionLost();

            wasConnected = isConnected;
        }
    }

    /// <summary>
    /// Gestiona la pérdida de conexión del dispositivo.
    /// Si se pierde la cpnexión volverá a la pantalla de inicio
    /// </summary>
    private void OnConnectionLost()
    {
        lostConnectionPanel.SetActive(true);
        Debug.LogWarning("[UnitySessionMonitor] Conexión perdida.");
        SceneManager.LoadScene(Scenes.Init.ToString());
    }

    /// <summary>
    /// Gestiona la reconexión del dispositivo.
    /// </summary>
    private void OnReconnect()
    {
        lostConnectionPanel.SetActive(false);
        Debug.Log("[UnitySessionMonitor] Conexión restablecida.");
    }
}
