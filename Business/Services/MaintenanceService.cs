using ATM.Shared.DTOs.Maintenance;
using BankAPI.DataAccess.Exceptions;
using BankAPI.DataAccess.Implementations;
using BankAPI.DataAccess.Interfaces;

namespace BankAPI.Business.Services
{
    public class MaintenanceService
    {
        private readonly IMaintenanceRepository _repo;
        private readonly Logger _logger;

        public MaintenanceService(IMaintenanceRepository repo)
        {
            _repo = repo;
            _logger = Logger.Instance;
        }

        // ── Roles ─────────────────────────────────────────────────────

        public RoleListResponse GetRoles()
            => _repo.GetRoles();

        public RoleDto CreateRole(CreateRoleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RoleName))
                throw new BankDatabaseException("El nombre del rol es obligatorio.", 50100);

            var result = _repo.CreateRole(request);
            _logger.LogInfo("Rol creado: " + request.RoleName);
            return result;
        }

        public RoleDto UpdateRole(UpdateRoleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RoleName))
                throw new BankDatabaseException("El nombre del rol es obligatorio.", 50100);

            var result = _repo.UpdateRole(request);
            _logger.LogInfo("Rol actualizado: " + request.RoleName);
            return result;
        }

        public void DeleteRole(int roleId)
        {
            _repo.DeleteRole(roleId);
            _logger.LogInfo("Rol eliminado. RoleId: " + roleId);
        }

        // ── AdminUsers ────────────────────────────────────────────────

        public AdminUserListResponse GetAdminUsers(string search)
            => _repo.GetAdminUsers(search);

        public MaintenanceResponse CreateAdminUser(CreateAdminUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new BankDatabaseException("El nombre de usuario es obligatorio.", 50110);

            if (string.IsNullOrWhiteSpace(request.Password) ||
                request.Password.Length < 6)
                throw new BankDatabaseException(
                    "La contraseña debe tener al menos 6 caracteres.", 50114);

            var result = _repo.CreateAdminUser(request);
            _logger.LogInfo("Usuario admin creado: " + request.Username);
            return result;
        }

        public MaintenanceResponse UpdateAdminUser(UpdateAdminUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new BankDatabaseException("El nombre de usuario es obligatorio.", 50110);

            if (!string.IsNullOrEmpty(request.NewPassword) &&
                request.NewPassword.Length < 6)
                throw new BankDatabaseException(
                    "La nueva contraseña debe tener al menos 6 caracteres.", 50114);

            var result = _repo.UpdateAdminUser(request);
            _logger.LogInfo("Usuario admin actualizado: " + request.Username);
            return result;
        }

        public void DeleteAdminUser(int adminId)
        {
            _repo.DeleteAdminUser(adminId);
            _logger.LogInfo("Usuario admin eliminado. AdminId: " + adminId);
        }
    }
}
