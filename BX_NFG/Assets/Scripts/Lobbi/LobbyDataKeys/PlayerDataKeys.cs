
namespace Assets.Scripts.Lobbi.Data
{
    /// <summary>
    /// Las claves de los datos que se almacenan de cada jugador al serializar.
    /// </summary>
    public static class PlayerDataKeys
    {
        /// <summary>
        /// Clave del id del usuario.
        /// </summary>
        public const string Id = "Id";
        /// <summary>
        /// Clave del nombre del usuario.
        /// </summary>
        public const string GameTag = "GameTag";
        /// <summary>
        /// Clave de si el usuario está listo para comenzar la partida.
        /// </summary>
        public const string IsReady = "IsReady";
        /// <summary>
        /// Clave del equipo al que perteneec el usuario.
        /// </summary>
        public const string PlayerTeam = "PlayerTeam";
    }
}
