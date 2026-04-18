using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ATM.Kiosk.Services.Configuration;
using ATM.Kiosk.Services.Exceptions;
using ATM.Shared.DTOs;
using Newtonsoft.Json;

namespace ATM.Kiosk.Services.Implementations
{
    /// <summary>
    /// Base para todos los clientes HTTP del ATM.
    /// Centraliza:
    ///   - La instancia compartida de HttpClient (debe ser estática)
    ///   - La gestión del JWT de sesión
    ///   - La serialización/deserialización JSON
    ///   - La conversión de respuestas del servidor en ApiException
    ///
    /// Por qué HttpClient es estático:
    ///   Crear una instancia por llamada agota los sockets del sistema
    ///   (TIME_WAIT). Una instancia compartida y reutilizada es el
    ///   patrón correcto para aplicaciones de larga duración como un ATM.
    /// </summary>
    public abstract class BaseApiClient
    {
        // Instancia compartida por todas las subclases
        private static readonly HttpClient _httpClient;

        // JWT de la sesión activa. Se establece en CreateSession
        // y se limpia en CloseSession.
        private static string _sessionToken;

        static BaseApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(KioskConfig.BankApiBaseUrl),
                Timeout     = TimeSpan.FromSeconds(KioskConfig.RequestTimeoutSeconds)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Establece el JWT tras autenticación exitosa.
        /// Llamado por AuthApiClient.CreateSession().
        /// </summary>
        public static void SetSessionToken(string token)
        {
            _sessionToken = token;
            _httpClient.DefaultRequestHeaders.Authorization =
                string.IsNullOrEmpty(token)
                    ? null
                    : new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>Limpia el JWT al cerrar sesión.</summary>
        public static void ClearSessionToken()
        {
            _sessionToken = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        // ── Métodos HTTP protegidos ───────────────────────────────────

        protected T Get<T>(string endpoint)
        {
            try
            {
                var response = _httpClient.GetAsync(endpoint).Result;
                return HandleResponse<T>(response);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(
                    $"Error de conexión con el servidor: {ex.Message}", ex);
            }
        }

        protected T Post<T>(string endpoint, object body)
        {
            try
            {
                var json     = JsonConvert.SerializeObject(body);
                var content  = new StringContent(json, Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync(endpoint, content).Result;
                return HandleResponse<T>(response);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(
                    $"Error de conexión con el servidor: {ex.Message}", ex);
            }
        }

        protected void PostVoid(string endpoint, object body)
        {
            try
            {
                var json     = JsonConvert.SerializeObject(body);
                var content  = new StringContent(json, Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync(endpoint, content).Result;
                HandleResponseVoid(response);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(
                    $"Error de conexión con el servidor: {ex.Message}", ex);
            }
        }

        // ── Manejo de respuestas ──────────────────────────────────────

        private T HandleResponse<T>(HttpResponseMessage response)
        {
            var raw    = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<ApiResult<T>>(raw);

            if (result == null)
                throw new ApiException("El servidor devolvió una respuesta vacía.");

            if (!result.Success)
                throw new ApiException(
                    result.Error?.Code    ?? 0,
                    result.Error?.Message ?? "Error desconocido del servidor.");

            return result.Data;
        }

        private void HandleResponseVoid(HttpResponseMessage response)
        {
            var raw    = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<ApiResult<object>>(raw);

            if (result != null && !result.Success)
                throw new ApiException(
                    result.Error?.Code    ?? 0,
                    result.Error?.Message ?? "Error desconocido del servidor.");
        }
    }
}
