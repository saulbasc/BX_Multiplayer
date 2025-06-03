using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class DynamicPrefabManager : Singleton<DynamicPrefabManager>
    {
        private NetworkPrefabsList networkPrefabsList;

        private void Awake()
        {
            networkPrefabsList = new NetworkPrefabsList();
        }

        public void AddPrefabToNetworkList(GameObject prefab)
        {
            var networkPrefab = new NetworkPrefab
            {
                Prefab = prefab
            };
            networkPrefabsList.Add(networkPrefab);
        }
    }
}
