
using Assets.Scripts.UI.MenuUI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Common
{
    public abstract class UIManagerBase : MonoBehaviour, IUIManager
    {
        [SerializeField] private PanelBase[] panels;

        private Dictionary<PanelType, PanelBase> _panelDictionary;
        private PanelBase currentPanel;

        private void Awake()
        {
            _panelDictionary = new Dictionary<PanelType, PanelBase>();
            foreach (var panel in panels)
            {
                panel.Initialize(this);
                panel.Hide();
                _panelDictionary[panel.PanelType] = panel;
            }
        }

        /// <summary>
        /// Lo que sucede en el start de las clases heredadas
        /// </summary>
        private void Start()
        {
            StartAction();
        }

        protected abstract void StartAction();

        /// <summary>
        /// Esconde el panel actual y muestra el nuevo panel
        /// </summary>
        /// <param name="type">El panel a mostrar</param>
        public void ShowPanel(PanelType type)
        {
            if (_panelDictionary.TryGetValue(type, out var panel))
            {
                if (currentPanel == panel) return;

                currentPanel?.Hide();
                currentPanel = panel;
                currentPanel.Show();
            }
            else
            {
                Debug.LogError($"Panel of type {type} not found.");
            }
        }

        /// <summary>
        /// Muestra el nuevo panel de manera flotante sin esconder el actual
        /// </summary>
        /// <param name="type">El panel a mostrar</param>
        public void AddFloatPanel(PanelType type)
        {
            if (_panelDictionary.TryGetValue(type, out var panel))
            {
                if (currentPanel == panel) return;

                panel.Show();
            }
        }

        /// <summary>
        /// Esconde el panel flotante seleccionado
        /// </summary>
        /// <param name="type">El panel a esconder</param>
        public void RemoveFloatPanel(PanelType type)
        {
            if (_panelDictionary.TryGetValue(type, out var panel))
            {
                if (currentPanel == panel) return;

                panel.Hide();
            }
        }
    }
}
