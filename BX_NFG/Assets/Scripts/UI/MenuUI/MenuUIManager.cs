using Assets.Scripts.UI.Common;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.MenuUI
{
    public class MenuUIManager : MonoBehaviour
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

        private void Start()
        {
            ShowPanel(PanelType.MainMenuPanel);
        }

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
            if(_panelDictionary.TryGetValue(type, out var panel))
            {
                if (currentPanel == panel) return;

                panel.Hide();
            }
        }
    }
}
