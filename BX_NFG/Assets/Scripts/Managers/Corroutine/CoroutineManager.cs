using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons;
using Assets.Scripts.Managers.Corroutine;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Clase encargada de gestionar las corrutinas del juego.
    /// </summary>
    public class CoroutineManager : Singleton<CoroutineManager>
    {
        private Dictionary<CoroutineIndentifier, Coroutine> _coroutineList = new ();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Inicia una corrutina y la asocia al identificador proporcionado. 
        /// Si ya existe una corrutina asociada a ese identificador, se detiene y reemplaza por la nueva.
        /// </summary>
        /// <param name="coroutineIdentifier">Identificador para la corrutina.</param>
        /// <param name="newCoroutine">Corrutina a procesar</param>
        public void StartTrackedCoroutine(CoroutineIndentifier coroutineIdentifier ,IEnumerator newCoroutine)
        {
            if (_coroutineList.TryGetValue(coroutineIdentifier, out Coroutine existing))
            {
                StopTrackedCoroutine(coroutineIdentifier);
            }

            Coroutine newTrackedCoroutine = StartCoroutine(newCoroutine);
            _coroutineList[coroutineIdentifier] = newTrackedCoroutine;
        }

        /// <summary>
        /// Detiene y elimina la corrutina asociada al identificador especificado.
        /// </summary>
        /// <param name="coroutineIdentifier">Indentificador de la corrutina a eliminar.</param>
        public void StopTrackedCoroutine(CoroutineIndentifier coroutineIdentifier)
        {
            if (_coroutineList.TryGetValue(coroutineIdentifier, out Coroutine coroutine))
            {
                StopCoroutine(coroutine);
                _coroutineList.Remove(coroutineIdentifier);
            }
        }

        /// <summary>
        /// Método encargado de detener todas las corrutinas activas y eliminarlas.
        /// </summary>
        public void StopAllTrackedCoroutines()
        {
            foreach (var (coroutineIdentifier, coroutine) in _coroutineList)
            {
                StopCoroutine(coroutine);
                _coroutineList.Remove(coroutineIdentifier);
            }
        }

        /// <summary>
        /// Comprueba si hay una corrutina en ejecución asociada al identificador especificado.
        /// </summary>
        /// <param name="key">Identificador de la corrutina a comprobar.</param>
        /// <returns></returns>
        public bool IsCoroutineRunning(CoroutineIndentifier corroutineIndentifier)
        {
            return _coroutineList.ContainsKey(corroutineIndentifier);
        }
    }
}
