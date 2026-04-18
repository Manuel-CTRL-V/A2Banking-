using ATM.Shared.DTOs.Maintenance;

namespace BankAPI.DataAccess.Interfaces
{
    public interface IMaintenanceRepository
    {
        // Roles
        RoleListResponse GetRoles();
        RoleDto CreateRole(CreateRoleRequest request);
        RoleDto UpdateRole(UpdateRoleRequest request);
        void DeleteRole(int roleId);
        void SetRolePermissions(int roleId, string permissionKeysCsv);

        // AdminUsers
        AdminUserListResponse GetAdminUsers(string search);
        MaintenanceResponse CreateAdminUser(CreateAdminUserRequest request);
        MaintenanceResponse UpdateAdminUser(UpdateAdminUserRequest request);
        void DeleteAdminUser(int adminId);
    }
}
