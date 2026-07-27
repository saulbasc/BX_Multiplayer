using System;
using Assets.Scripts.Commons;
using UnityEngine;

namespace Assets.Scripts.Handlers
{
    /// <summary>
    /// Encargado de procesar excepciones.
    /// </summary>
    public class ExceptionHandler : DefaultSingleton<ExceptionHandler>
    {
        /// <summary>
        /// Recoge la excepción y la muestra mediante debug.
        /// </summary>
        /// <param name="e">Excepción a mostrar</param>
        public void HandleException(Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }
}
