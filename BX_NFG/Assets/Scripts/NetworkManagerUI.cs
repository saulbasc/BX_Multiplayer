using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button serverButton;
    [SerializeField] private Button clientButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(OnHostButtonClicked);
        serverButton.onClick.AddListener(OnServerButtonClicked);
        clientButton.onClick.AddListener(OnClientButtonClicked);
    }

    private void OnHostButtonClicked()
    {
        NetworkManager.Singleton.StartHost();
    }

    private void OnServerButtonClicked()
    {
        NetworkManager.Singleton.StartServer();
    }

    private void OnClientButtonClicked()
    {
        NetworkManager.Singleton.StartClient();
    }
}
