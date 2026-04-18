namespace ATM.Shared.DTOs
{
    /// <summary>
    /// Envoltorio estándar para todas las respuestas del BankAPI.
    /// El servidor siempre responde con esta estructura.
    /// El cliente siempre espera esta estructura.
    ///
    /// Éxito:  { Success: true,  Data: {...}, Error: null }
    /// Error:  { Success: false, Data: null,  Error: { Code: 50033, Message: "PIN incorrecto" } }
    ///
    /// Esto evita que el cliente tenga que parsear mensajes de error
    /// de HTTP status codes o strings. Puede hacer switch(result.Error.Code).
    /// </summary>
    public class ApiResult<T>
    {
        public bool     Success { get; set; }
        public T        Data    { get; set; }
        public ApiError Error   { get; set; }

        public static ApiResult<T> Ok(T data) =>
            new ApiResult<T> { Success = true, Data = data };

        public static ApiResult<T> Fail(int code, string message) =>
            new ApiResult<T>
            {
                Success = false,
                Error   = new ApiError { Code = code, Message = message }
            };
    }

    public class ApiError
    {
        /// <summary>
        /// Espeja los códigos THROW de los stored procedures (50001-50099).
        /// El cliente puede tomar decisiones basadas en el código
        /// sin parsear strings de mensajes.
        /// </summary>
        public int    Code    { get; set; }
        public string Message { get; set; }
    }
}
