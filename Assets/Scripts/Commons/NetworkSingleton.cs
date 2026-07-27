using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class NetworkSingleton<T> : NetworkBehaviour where T : Component
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T[] objects = FindObjectsByType<T>(FindObjectsSortMode.None);
                    if (objects.Length > 0)
                    {
                        T foundInstance = objects[0];
                        instance = foundInstance;
                    }
                    else
                    {
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        instance = go.AddComponent<T>();
                        DontDestroyOnLoad(go);
                    }
                }
                return instance;
            }
        }
    }
}
