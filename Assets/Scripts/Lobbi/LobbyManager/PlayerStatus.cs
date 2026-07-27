
using Assets.Scripts.Commons;

namespace Assets.Scripts.Lobbi.Logic
{
    /// <summary>
    /// Guarda el status en la partida del jugador local.
    /// </summary>
    public class PlayerStatus : DefaultSingleton<PlayerStatus>
    {
        /// <summary>
        /// Indica si el jugador local ha entrado en el partido.
        /// </summary>
        public bool JoinedGame {  get; set; }
        /// <summary>
        /// Indica si el jugador local está jugando el partido.
        /// </summary>
        public bool InGame {  get; set; }
    }
}
