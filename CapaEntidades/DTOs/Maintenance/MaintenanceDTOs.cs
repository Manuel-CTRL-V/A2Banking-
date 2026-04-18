using System;
using System.Collections.Generic;

namespace ATM.Shared.DTOs.Maintenance
{
    public class PermissionDto
    {
        public int PermissionId { get; set; }
        public string PermissionKey { get; set; }
        public string Description { get; set; }
        public bool IsGranted { get; set; } // true si el rol lo tiene
    }

    // Roles

    public class RoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Descriptions { get; set; }
        public List<string> Permissions { get; set; }
            = new List<string>();
    }

    public class RoleListResponse
    {
        public List<RoleDto> Roles { get; set; }
            = new List<RoleDto>();
        public List<PermissionDto> AllPermissions { get; set; }
            = new List<PermissionDto>();
    }

    public class CreateRoleRequest
    {
        public string RoleName { get; set; }
        public string Descriptions { get; set; }
        public List<string> PermissionKeys { get; set; }
            = new List<string>();
    }

    public class UpdateRoleRequest
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Descriptions { get; set; }
        public List<string> PermissionKeys { get; set; }
            = new List<string>();
    }

    // AdminUsers

    public class AdminUserDto
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AdminUserListResponse
    {
        public List<AdminUserDto> Users { get; set; }
            = new List<AdminUserDto>();
    }

    public class CreateAdminUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }  // texto plano — se hashea en el servicio
        public int RoleId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateAdminUserRequest
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        // Vacío = no cambiar contraseña
        public string NewPassword { get; set; }
    }
    public class MaintenanceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }
    }
}
