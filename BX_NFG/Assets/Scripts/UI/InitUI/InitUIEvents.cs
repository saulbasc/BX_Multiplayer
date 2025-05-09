
using System;
using Assets.Scripts.Commons;

namespace Assets.Scripts.UI.InitUI
{
    /// <summary>
    /// Eventos que lanza la interfaz inicial de la aplicación.
    /// </summary>
    public class InitUIEvents : DefaultSingleton<InitUIEvents>
    {
        public event Action<string> onConfirmButtonClicked;
        /// <summary>
        /// Evento indicador de que el usuario ha confirmado su nombre en el panel inicial
        /// </summary>
        public void RaiserConfirmButtonClicked(string userName) => onConfirmButtonClicked?.Invoke(userName);
    }
}
