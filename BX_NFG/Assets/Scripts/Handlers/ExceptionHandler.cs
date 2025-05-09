using System;
using UnityEngine;

namespace Assets.Scripts.Handlers
{
    public class ExceptionHandler
    {
        private static ExceptionHandler instance;

        private ExceptionHandler() { }

        public static ExceptionHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExceptionHandler();
                }
                return instance;
            }
        }

        /// <summary>
        /// Método que recoge la excepción y la muestra mediante debug.
        /// </summary>
        /// <param name="e">Excepción a mostrar</param>
        public void HandleException(Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }
}
