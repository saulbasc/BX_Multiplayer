using TMPro;
using UnityEngine;

namespace Assets.Scripts.Lobbi.Players
{
    /// <summary>
    /// Clase dedicada a mostrar la información de los jugadores en su respectivo panel de la Lobby.
    /// </summary>
    public class PlayerPanelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;

        /// <summary>
        /// Establece el nombre y el estatus del jugador en el panel.
        /// </summary>
        /// <param name="playerName">El nombre del jugador</param>
        /// <param name="isReady">True si el jugador está listo.</param>
        public void SetPlayerNameAndStatus(string playerName, bool isReady)
        {
            if(playerNameText != null)
            {
                playerNameText.text = playerName;
                playerNameText.color = isReady ? Color.green : Color.red;
            }
        }
    }
}
