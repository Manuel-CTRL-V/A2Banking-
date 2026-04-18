using ATM.Shared.DTOs;
using ATM.Shared.DTOs.BackOffice;
using ATM.Shared.DTOs.Maintenance;
using BackOffice.Services.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;

namespace BackOffice.Services.Implementations
{
    /// <summary>
    /// Cliente HTTP para todos los endpoints del back-office.
    /// Reutiliza la misma instancia de HttpClient (patrón correcto).
    /// </summary>
    public class BackOfficeApiClient
    {
        private static readonly HttpClient _http;
        private static string _token;
        static BackOfficeApiClient()
        {
            var handler = new HttpClientHandler
            {
                UseDefaultCredentials = false
            };

            _http = new HttpClient(handler)
            {
                BaseAddress = new Uri(BackOfficeConfig.BankApiBaseUrl),
                Timeout = TimeSpan.FromSeconds(
                                  BackOfficeConfig.RequestTimeoutSeconds)
            };
            _http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _http.DefaultRequestHeaders.ConnectionClose = true;
        }

        public static void SetToken(string token)
        {
            _token = token;
            _http.DefaultRequestHeaders.Authorization =
                string.IsNullOrEmpty(token)
                    ? null
                    : new AuthenticationHeaderValue("Bearer", token);
        }

        public static void ClearToken()
        {
            _token = null;
            _http.DefaultRequestHeaders.Authorization = null;
        }

        // Admin auth

        public GetAdminSaltResponse GetAdminSalt(string username)
            => Get<GetAdminSaltResponse>("backoffice/admin/salt/" + username);

        public AdminLoginResponse AdminLogin(AdminLoginRequest request)
            => Post<AdminLoginResponse>("backoffice/admin/login", request);

        // Simpsons 

        public System.Collections.Generic.List<SimpsonsCharacterDto>
            SearchCharacters(string search)
        {
            var url = string.IsNullOrEmpty(search)
                ? "backoffice/characters"
                : "backoffice/characters?search=" + Uri.EscapeDataString(search);

            return Get<System.Collections.Generic.List<SimpsonsCharacterDto>>(url);
        }

        // Cuentas 

        public CreateAccountResponse CreateAccount(
            CreateAccountRequest request, int adminId)
            => Post<CreateAccountResponse>(
                "backoffice/accounts/create?adminId=" + adminId, request);

        public EnrollBiometricResponse EnrollBiometric(
            EnrollBiometricRequest request, int adminId)
            => Post<EnrollBiometricResponse>(
                "backoffice/accounts/enroll?adminId=" + adminId, request);

        public AccountListResponse GetAccounts()
            => Get<AccountListResponse>("backoffice/accounts");

        public UpdateAccountStatusResponse UpdateAccountStatus(
            UpdateAccountStatusRequest request, int adminId)
            => Put<UpdateAccountStatusResponse>(
                "backoffice/accounts/status?adminId=" + adminId, request);

        // Logs

        public AuditLogResponse GetLogs(AuditLogRequest request)
            => Post<AuditLogResponse>("backoffice/logs", request);
        public StatisticsResponse GetStatistics(StatisticsRequest request)
            => Post<StatisticsResponse>("backoffice/statistics", request);


        // HTTP helpers 

        private T Get<T>(string url)
        {
            try
            {
                var response = _http.GetAsync(url).Result;
                return HandleResponse<T>(response);
            }
            catch (Exception ex) when (!(ex is BackOfficeApiException))
            {
                throw new BackOfficeApiException(
                    "Error de conexión: " + ex.Message, ex);
            }
        }

        private T Post<T>(string url, object body)
        {
            try
            {
                var json     = JsonConvert.SerializeObject(body);
                var content  = new StringContent(json, Encoding.UTF8, "application/json");
                var response = _http.PostAsync(url, content).Result;
                return HandleResponse<T>(response);
            }
            catch (Exception ex) when (!(ex is BackOfficeApiException))
            {
                throw new BackOfficeApiException(
                    "Error de conexión: " + ex.Message, ex);
            }
        }

        private T Put<T>(string url, object body)
        {
            try
            {
                var json     = JsonConvert.SerializeObject(body);
                var content  = new StringContent(json, Encoding.UTF8, "application/json");
                var response = _http.PutAsync(url, content).Result;
                return HandleResponse<T>(response);
            }
            catch (Exception ex) when (!(ex is BackOfficeApiException))
            {
                throw new BackOfficeApiException(
                    "Error de conexión: " + ex.Message, ex);
            }
        }
        private void Delete(string url)
        {
            try
            {
                var response = _http.DeleteAsync(url).Result;
                HandleResponse<object>(response);
            }
            catch (Exception ex) when (!(ex is BackOfficeApiException))
            {
                throw new BackOfficeApiException("Error de conexión: " + ex.Message, ex);
            }
        }

        private T HandleResponse<T>(HttpResponseMessage response)
        {
            var raw    = response.Content.ReadAsStringAsync().Result;

            System.Diagnostics.Debug.WriteLine("RAW: " + raw); // ← agregar esto

            var result = JsonConvert.DeserializeObject<ApiResult<T>>(raw);
            System.Diagnostics.Debug.WriteLine("URL: " + _http.BaseAddress );
            System.Diagnostics.Debug.WriteLine("STATUS: " + response.StatusCode);
            System.Diagnostics.Debug.WriteLine("RAW: " + raw);

            if (result == null)
                throw new BackOfficeApiException("El servidor devolvió una respuesta vacía.");

            if (!result.Success)
                throw new BackOfficeApiException(
                    result.Error?.Code ?? 0,
                    result.Error?.Message ?? "Error desconocido.");

            return result.Data;
        }

        // ── Agregar al final de BackOfficeApiClient ───────────────────
        // Roles
        public RoleListResponse GetRoles()
            => Get<RoleListResponse>("maintenance/roles");

        public RoleDto CreateRole(CreateRoleRequest request)
            => Post<RoleDto>("maintenance/roles", request);

        public RoleDto UpdateRole(UpdateRoleRequest request)
            => Put<RoleDto>("maintenance/roles", request);

        public void DeleteRole(int roleId)
            => Delete("maintenance/roles/" + roleId);

        // AdminUsers
        public AdminUserListResponse GetAdminUsers(string search = "")
        {
            var url = string.IsNullOrEmpty(search)
                ? "maintenance/users"
                : "maintenance/users?search=" + Uri.EscapeDataString(search);
            return Get<AdminUserListResponse>(url);
        }

        public MaintenanceResponse CreateAdminUser(CreateAdminUserRequest request)
            => Post<MaintenanceResponse>("maintenance/users", request);

        public MaintenanceResponse UpdateAdminUser(UpdateAdminUserRequest request)
            => Put<MaintenanceResponse>("maintenance/users", request);

        public void DeleteAdminUser(int adminId)
            => Delete("maintenance/users/" + adminId);
    }
}
