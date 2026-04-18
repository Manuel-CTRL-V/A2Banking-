using ATM.Shared.DTOs.BackOffice;

namespace BankAPI.DataAccess.Interfaces
{
    public interface IAccountManagementRepository
    {
        // Admin auth
        GetAdminSaltResponse   GetAdminSalt(string username);
        AdminLoginResponse     AdminLogin(AdminLoginRequest request);

        // Cuentas 
        CreateAccountResponse  CreateAccount(CreateAccountRequest request, int adminId);
        EnrollBiometricResponse EnrollBiometric(EnrollBiometricRequest request, int adminId);
        AccountListResponse    GetAccounts();
        UpdateAccountStatusResponse UpdateAccountStatus(
            UpdateAccountStatusRequest request, int adminId);

        //Logs 
        AuditLogResponse       GetLogs(AuditLogRequest request);
        StatisticsResponse GetStatistics(StatisticsRequest request);

    }
}
