using System;
using ATM.Kiosk.Business.Exceptions;
using ATM.Kiosk.Services.Implementations;
using ATM.Shared.DTOs.Auth;
using ATM.Shared.Models;

namespace ATM.Kiosk.Business.Auth
{
    /// <summary>
    /// Gestiona el estado de la sesión activa del cliente en memoria.
    /// Es el único lugar del sistema que sabe si hay una sesión abierta
    /// y quién es el cliente autenticado.
    ///
    /// Singleton porque solo puede haber una sesión activa por ATM
    /// físico en cualquier momento — un cliente a la vez.
    ///
    /// Responsabilidades:
    ///   - Crear y destruir la sesión en memoria
    ///   - Detectar inactividad y expirar la sesión
    ///   - Actualizar el balance local tras cada operación
    ///   - Proveer el SessionId y ATM_Identifier para cada request
    /// </summary>
    public sealed class SessionManager
    {
        private static readonly Lazy<SessionManager> _instance =
            new Lazy<SessionManager>(() => new SessionManager());

        public static SessionManager Instance { get { return _instance.Value; } }

        // Timeout de inactividad — 2 minutos como ATM real
        private static readonly TimeSpan IdleTimeout = TimeSpan.FromMinutes(2);

        private ActiveSession _current;
        private readonly object _lock = new object();

        private SessionManager() { }

        // ── API pública ───────────────────────────────────────────────

        /// <summary>Sesión actual. Null si no hay cliente autenticado.</summary>
        public ActiveSession Current
        {
            get
            {
                lock (_lock) { return _current; }
            }
        }

        public bool HasActiveSession
        {
            get
            {
                lock (_lock)
                {
                    return _current != null && !IsExpired(_current);
                }
            }
        }

        /// <summary>
        /// Crea la sesión en memoria tras autenticación completa.
        /// Llamado por AuthService después de PIN + huella verificados.
        /// </summary>
        public void Open(CreateSessionResponse serverResponse,
                         AuthStartResponse authData,
                         string atmIdentifier)
        {
            lock (_lock)
            {
                _current = new ActiveSession
                {
                    SessionId = serverResponse.SessionId,
                    Token = serverResponse.Token,
                    AccountId = authData.AccountId,
                    HolderName = authData.HolderName,
                    ATM_Identifier = atmIdentifier,
                    FingerprintTemplate = authData.FingerprintTemplate,
                    PIN_Salt = authData.PIN_Salt,
                    StartedAt = DateTime.Now,
                    LastActivityAt = DateTime.Now,
                    CurrentBalance = 0
                };

                // Establecer JWT en todos los clientes HTTP
                BaseApiClient.SetSessionToken(serverResponse.Token);
            }
        }

        /// <summary>
        /// Cierra y limpia la sesión en memoria.
        /// Llamado por AuthService al logout o timeout.
        /// </summary>
        public void Close()
        {
            lock (_lock)
            {
                _current = null;
                BaseApiClient.ClearSessionToken();
            }
        }

        /// <summary>
        /// Registra actividad del usuario — reinicia el contador de timeout.
        /// Debe llamarse en cada interacción del usuario con la UI.
        /// </summary>
        public void RecordActivity()
        {
            lock (_lock)
            {
                if (_current != null)
                    _current.RefreshActivity();
            }
        }

        /// <summary>
        /// Verifica si la sesión expiró por inactividad.
        /// La capa de presentación consulta esto periódicamente.
        /// </summary>
        public bool CheckExpired()
        {
            lock (_lock)
            {
                return _current != null && IsExpired(_current);
            }
        }

        /// <summary>
        /// Actualiza el balance local tras una operación exitosa.
        /// Evita tener que consultar GetBalance después de cada transacción.
        /// </summary>
        public void UpdateBalance(decimal newBalance)
        {
            lock (_lock)
            {
                if (_current != null)
                    _current.CurrentBalance = newBalance;
            }
        }

        /// <summary>
        /// Devuelve la sesión activa o lanza BusinessException si no hay una
        /// o si expiró. Usado por todas las strategies antes de operar.
        /// </summary>
        public ActiveSession RequireActiveSession()
        {
            lock (_lock)
            {
                if (_current == null)
                    throw new BusinessException(
                        BusinessErrorCode.SessionExpired,
                        "No hay sesión activa. Por favor autentíquese.");

                if (IsExpired(_current))
                {
                    Close();
                    throw new BusinessException(
                        BusinessErrorCode.SessionExpired,
                        "La sesión expiró por inactividad.");
                }

                _current.RefreshActivity();
                return _current;
            }
        }

        // ── Privados ──────────────────────────────────────────────────

        private static bool IsExpired(ActiveSession session)
        {
            return session.IdleTime > IdleTimeout;
        }
    }
}
