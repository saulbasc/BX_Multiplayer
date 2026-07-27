using System;
using Assets.Scripts.Commons;

namespace Assets.Scripts.Init
{
    /// <summary>
    /// Eventos que se lanzan al iniciar la aplicación.
    /// </summary>
    public class InitEventManager : DefaultSingleton<InitEventManager>
    {
        public event Action OnUserNotRegistered;
        public event Action OnUserRegisteredSuccessfully;

        /// <summary>
        /// Evento indicador de que el usuario no está registrado en la base de datos 
        /// </summary>
        public void RaiseUserNotRegistered() => OnUserNotRegistered?.Invoke();

        /// <summary>
        /// Evento indicador de que el usuario se ha registrado en la base de datos con éxito
        /// </summary>
        public void RaiseUserRegisteredSuccessfully() => OnUserRegisteredSuccessfully?.Invoke();
    }
}
