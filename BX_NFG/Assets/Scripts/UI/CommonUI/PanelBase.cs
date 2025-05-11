using Assets.Scripts.GameManager.GameEvents.UI;
using Assets.Scripts.UI.Common;
using UnityEngine;

namespace Assets.Scripts.UI.MenuUI
{
    /// <summary>
    /// Clase abstracta de la que heredan los paneles de la interfaz del juego
    /// </summary>
    public abstract class PanelBase : MonoBehaviour, IPanel
    {
        /// <summary>
        /// Gestiona las acciones de los paneles
        /// </summary>
        protected IUIManager menuManager;

        public abstract PanelType PanelType { get; }
        /// <summary>
        /// Inicializa las acciones disponibles para el panel
        /// </summary>
        /// <param name="manager">El gestor de paneles</param>
        public abstract void Initialize(IUIManager manager);

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}