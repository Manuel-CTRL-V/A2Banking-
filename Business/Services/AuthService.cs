using System;
using System.Text;
using ATM.Shared.DTOs.Auth;
using BankAPI.DataAccess.Interfaces;
using BankAPI.DataAccess.Exceptions;
using BankAPI.DataAccess.Implementations;

namespace BankAPI.Business.Services
{
    /// <summary>
    /// Lógica de negocio de autenticación en el servidor.
    /// Recibe los requests del ATM y coordina con el repositorio.
    ///
    /// JWT simplificado para desarrollo:
    ///   Base64(accountId:sessionId:timestamp)
    ///   En producción reemplazar por System.IdentityModel.Tokens.Jwt
    /// </summary>
    public class AuthService
    {
        private readonly IAccountRepository _repo;
        private readonly Logger             _logger;

        public AuthService(IAccountRepository repo)
        {
            _repo   = repo;
            _logger = Logger.Instance;
        }

        public AuthStartResponse GetAccountForAuth(int accountId)
        {
            _logger.LogInfo("Solicitud de auth para cuenta: " + accountId);

            var response = _repo.GetAccountForAuth(accountId);

            if (response.Status != ATM.Shared.Enums.AccountStatus.Active)
                throw new BankDatabaseException(
                    "La cuenta no está activa.", 50032);

            return response;
        }

        public VerifyPinResponse VerifyPin(VerifyPinRequest request)
        {
            // El SP lanza excepción si el PIN no coincide
            _repo.VerifyPIN(request);

            _logger.LogInfo("PIN verificado.", accountId: request.AccountId);

            return new VerifyPinResponse
            {
                Verified = true,
                Message  = "PIN verificado correctamente."
            };
        }

        public CreateSessionResponse CreateSession(CreateSessionRequest request)
        {
            var dbResponse = _repo.CreateSession(request);

            // Generar token simplificado
            // En producción: usar JWT con firma HMAC-SHA256
            var payload = request.AccountId + ":" +
                          dbResponse.SessionId + ":" +
                          DateTime.UtcNow.Ticks;

            var token = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(payload));

            _logger.LogInfo(
                "Sesión creada. SessionId: " + dbResponse.SessionId,
                accountId: request.AccountId,
                sessionId: dbResponse.SessionId);

            return new CreateSessionResponse
            {
                SessionId = dbResponse.SessionId,
                Token     = token
            };
        }

        public void CloseSession(CloseSessionRequest request)
        {
            _repo.CloseSession(request.SessionId, request.TerminationReason);
            _logger.LogInfo("Sesión cerrada. Razón: " + request.TerminationReason,
                            sessionId: request.SessionId);
        }
    }
}
