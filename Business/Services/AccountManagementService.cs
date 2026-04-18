using System;
using System.Collections.Generic;
using ATM.Shared.DTOs.BackOffice;
using BankAPI.DataAccess.Exceptions;
using BankAPI.DataAccess.Implementations;
using BankAPI.DataAccess.Interfaces;

namespace BankAPI.Business.Services
{
    public class AccountManagementService
    {
        private readonly IAccountManagementRepository _repo;
        private readonly SimpsonsApiService _simpsons;
        private readonly Logger _logger;

        public AccountManagementService(IAccountManagementRepository repo)
        {
            _repo = repo;
            _simpsons = new SimpsonsApiService();
            _logger = Logger.Instance;
        }

        // Simpsons API 

        public List<SimpsonsCharacterDto> SearchCharacters(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return _simpsons.GetAll();

            return _simpsons.Search(term);
        }

        // Crear cuenta

        public CreateAccountResponse CreateAccount(
            CreateAccountRequest request, int adminId)
        {
            // Validar que el personaje exista en la API antes de crear
            if (!_simpsons.Exists(request.ApiCharacterId))
                throw new BankDatabaseException(
                    "El personaje no existe en la Simpsons API.", 50010);

            var response = _repo.CreateAccount(request, adminId);

            _logger.LogInfo(
                "Cuenta creada. AccountId: " + response.AccountId +
                ", Titular: " + request.FullName,
                sessionId: adminId);

            return response;
        }

        // Enrollment biométrico

        public EnrollBiometricResponse EnrollBiometric(
            EnrollBiometricRequest request, int adminId)
        {
            if (request.FingerprintTemplate == null ||
                request.FingerprintTemplate.Length == 0)
                throw new BankDatabaseException(
                    "El template biométrico no puede estar vacío.", 50022);

            if (request.PIN_Hash == null || request.PIN_Hash.Length == 0)
                throw new BankDatabaseException(
                    "El hash del PIN no puede estar vacío.", 50022);

            var response = _repo.EnrollBiometric(request, adminId);

            _logger.LogInfo(
                "Enrollment biométrico completado. AccountId: " + request.AccountId,
                accountId: request.AccountId,
                sessionId:   adminId);

            return response;
        }

        // Gestión de cuentas

        public AccountListResponse GetAccounts()
        {
            return _repo.GetAccounts();
        }

        public UpdateAccountStatusResponse UpdateStatus(
            UpdateAccountStatusRequest request, int adminId)
        {
            var response = _repo.UpdateAccountStatus(request, adminId);

            _logger.LogInfo(
                "Estado de cuenta actualizado. AccountId: " + request.AccountId +
                ", Nuevo estado: " + response.NewStatus,
                accountId: request.AccountId,
                sessionId:   adminId);

            return response;
        }

        // Logs

        public AuditLogResponse GetLogs(AuditLogRequest request)
        {
            return _repo.GetLogs(request);
        }
        public StatisticsResponse GetStatistics(StatisticsRequest request)
        {
            return _repo.GetStatistics(request);
        }
    }
}
