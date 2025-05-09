using System;
using System.Threading.Tasks;

namespace Assets.Scripts.Handlers
{
    /// <summary>
    /// Clase para gestionar las excepciones de los métodos asíncronos.
    /// </summary>
    public static class SafeAsyncFunctionsHandler
    {
        /// <summary>
        /// Método que permite envolver una funcion asíncrona y recoger sus excepciones.
        /// </summary>
        /// <param name="func">Función asíncrona a ejecutar</param>
        /// <returns></returns>
        public static async Task ExecuteAsync(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (Exception e)
            {
                ExceptionHandler.Instance.HandleException(e); 
            }
        }

        /// <summary>
        /// Ejecuta una función asíncrona que retorna un valor y captura cualquier excepción que se produzca.
        /// </summary>
        /// <typeparam name="T">Tipo del valor que retorna la función.</typeparam>
        /// <param name="func">Función asíncrona a ejecutar.</param>
        /// <param name="defaultValue">Valor por defecto a retornar cuando se produce la excepción. (Opcional)</param>
        /// <returns>Valor retornado por la función o <c>default</c> si ocurre una excepción.</returns>
        public static async Task<T> ExecuteAsync<T>(Func<Task<T>> func, T defaultValue = default)
        {
            try
            {
                return await func();
            }
            catch (Exception e)
            {
                ExceptionHandler.Instance.HandleException(e);
                return defaultValue;
            }
        }
    }
}