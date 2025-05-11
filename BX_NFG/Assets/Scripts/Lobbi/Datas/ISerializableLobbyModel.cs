using System.Collections.Generic;

namespace Assets.Scripts.Lobbi.Datas
{
    /// <summary>
    /// Implementa métodos para serializar y deserializar diccionarios.
    /// </summary>
    /// <typeparam name="TDataObject">El tipo del valor del diccionario</typeparam>
    public interface ISerializableLobbyModel<TDataObject>
    {
        /// <summary>
        /// Recoge un diccionario definido y lo transforma a los atributos de la clase que lo hereda
        /// </summary>
        /// <param name="data">El diccionario a transformar</param>
        void DeserializeFromDictionary(Dictionary<string, TDataObject> data);
        /// <summary>
        /// Transforma los atributos del propio objeto en un diccionario
        /// </summary>
        /// <returns>El diccionario obtenido</returns>
        Dictionary<string, string> SerializeObjectToDictionary();
    }
}
