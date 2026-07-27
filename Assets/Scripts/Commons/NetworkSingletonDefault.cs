using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class NetworkSingletonDefault<T> : NetworkBehaviour where T : Component
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T[] objects = FindObjectsByType<T>(FindObjectsSortMode.None);
                    foreach (var obj in objects)
                    {
                        if (obj is NetworkBehaviour nb && nb.IsSpawned)
                        {
                            instance = obj;
                            break;
                        }
                    }
                    if (instance == null)
                    {
                        Debug.LogError($"No network-spawned instance of {typeof(T).Name} found!");
                    }
                }
                return instance;
            }
        }
    }
}
