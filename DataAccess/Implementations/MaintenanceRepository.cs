using System;
using System.Security.Cryptography;
using System.Text;
using ATM.Shared.DTOs.Maintenance;
using BankAPI.DataAccess.Exceptions;
using BankAPI.DataAccess.Helpers;
using BankAPI.DataAccess.Interfaces;

namespace BankAPI.DataAccess.Implementations
{
    public class MaintenanceRepository : BaseRepository, IMaintenanceRepository
    {
        // Roles

        public RoleListResponse GetRoles()
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder.For("sp_GetRoles", conn).Build())
                using (var r = cmd.ExecuteReader())
                {
                    var response = new RoleListResponse();

                    // Result set 1: roles
                    while (r.Read())
                        response.Roles.Add(new RoleDto
                        {
                            RoleId = Convert.ToInt32(r.GetValue(r.GetOrdinal("RoleId"))),
                            RoleName = r.GetString("RoleName"),
                            Descriptions = r.GetString("Descriptions")
                        });

                    // Result set 2: permisos por rol
                    r.NextResult();
                    while (r.Read())
                    {
                        object roleIdValue = r["RoleId"];
                        int roleId = Convert.ToInt32(roleIdValue);
                        string key = r.GetString("PermissionKey");

                        var role = response.Roles.Find(x => x.RoleId == roleId);
                        role?.Permissions.Add(key);
                    }

                    // También cargar catálogo completo para el CheckedListBox
                    r.Close();
                    using (var cmdP = SqlCommandBuilder
                        .For("sp_GetAllPermissions", conn).Build())
                    using (var rP = cmdP.ExecuteReader())
                    {
                        while (rP.Read())
                            response.AllPermissions.Add(new PermissionDto
                            {
                                PermissionId = rP.GetInt("PermissionId"),
                                PermissionKey = rP.GetString("PermissionKey"),
                                Description = rP.GetString("Description")
                            });
                    }

                    return response;
                }
            });
        }

        public void SetRolePermissions(int roleId, string permissionKeysCsv)
        {
            Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_SetRolePermissions", conn)
                    .With("@RoleId", roleId)
                    .With("@PermissionKeys", permissionKeysCsv)
                    .Build())
                using (var r = cmd.ExecuteReader()) { }
            });
        }

        public RoleDto CreateRole(CreateRoleRequest request)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_CreateRole", conn)
                    .With("@RoleName", request.RoleName)
                    .WithNullable("@Descriptions", request.Descriptions)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException("No se pudo crear el rol.");

                    var role = new RoleDto
                    {
                        RoleId = Convert.ToInt32(r.GetValue(r.GetOrdinal("RoleId"))),
                        RoleName = r.GetString("RoleName"),
                        Descriptions = r.GetString("Descriptions")
                    };
                    r.Close();

                    // Guardar permisos seleccionados
                    var csv = string.Join(",", request.PermissionKeys);
                    SetRolePermissions(role.RoleId, csv);
                    role.Permissions = new System.Collections.Generic.List<string>(
                        request.PermissionKeys);
                    return role;
                }
            });
        }

        public RoleDto UpdateRole(UpdateRoleRequest request)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_UpdateRole", conn)
                    .With("@RoleId", Convert.ToByte(request.RoleId))
                    .With("@RoleName", request.RoleName)
                    .WithNullable("@Descriptions", request.Descriptions)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException("No se pudo actualizar el rol.");

                    var role = new RoleDto
                    {
                        RoleId = Convert.ToInt32(r.GetValue(r.GetOrdinal("RoleId"))),
                        RoleName = r.GetString("RoleName"),
                        Descriptions = r.GetString("Descriptions")
                    };
                    r.Close();

                    // Reemplazar permisos
                    var csv = string.Join(",", request.PermissionKeys);
                    SetRolePermissions(role.RoleId, csv);
                    role.Permissions = new System.Collections.Generic.List<string>(
                        request.PermissionKeys);
                    return role;
                }
            });
        }

        public void DeleteRole(int roleId)
        {
            Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_DeleteRole", conn)
                    .With("@RoleId", Convert.ToByte(roleId))
                    .Build())
                    cmd.ExecuteNonQuery();
            });
        }

        // AdminUsers

        public AdminUserListResponse GetAdminUsers(string search)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_GetAdminUsers", conn)
                    .WithNullable("@Search", string.IsNullOrEmpty(search) ? null : search)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    var list = new AdminUserListResponse();
                    while (r.Read())
                        list.Users.Add(new AdminUserDto
                        {
                            AdminId = r.GetInt("AdminId"),
                            Username = r.GetString("Username"),
                            RoleName = r.GetString("RoleName"),
                            RoleId = Convert.ToInt32(r.GetValue(r.GetOrdinal("RoleId"))),
                            IsActive = r.GetBool("IsActive"),
                            CreatedAt = r.GetDateTime("CreatedAt")
                        });
                    return list;
                }
            });
        }

        public MaintenanceResponse CreateAdminUser(CreateAdminUserRequest request)
        {
            return Execute(conn =>
            {
                var salt = GenerateSalt();
                var hash = HashPassword(request.Password, salt);

                using (var cmd = SqlCommandBuilder
                    .For("sp_CreateAdminUser", conn)
                    .With("@Username", request.Username)
                    .With("@PasswordHash", hash)
                    .With("@PasswordSalt", salt)
                    .With("@RoleId", Convert.ToByte(request.RoleId))
                    .With("@IsActive", request.IsActive ? 1 : 0)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException("No se pudo crear el usuario.");
                    return new MaintenanceResponse
                    {
                        Success = true,
                        Id = Convert.ToInt32(r.GetValue(r.GetOrdinal("AdminId"))),
                        Message = "Usuario '" + request.Username + "' creado correctamente."
                    };
                }
            });
        }

        public MaintenanceResponse UpdateAdminUser(UpdateAdminUserRequest request)
        {
            return Execute(conn =>
            {
                byte[] hash = null;
                byte[] salt = null;

                if (!string.IsNullOrEmpty(request.NewPassword))
                {
                    salt = GenerateSalt();
                    hash = HashPassword(request.NewPassword, salt);
                }

                using (var cmd = SqlCommandBuilder
                    .For("sp_UpdateAdminUser", conn)
                    .With("@AdminId", request.AdminId)
                    .With("@Username", request.Username)
                    .With("@RoleId", Convert.ToByte(request.RoleId))
                    .With("@IsActive", request.IsActive ? 1 : 0)
                    .WithMax("@PasswordHash", hash)
                    .WithMax("@PasswordSalt", salt)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException("No se pudo actualizar el usuario.");
                    return new MaintenanceResponse
                    {
                        Success = true,
                        Id = r.GetInt("AdminId"),
                        Message = "Usuario actualizado correctamente."
                    };
                }
            });
        }

        public void DeleteAdminUser(int adminId)
        {
            Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_DeleteAdminUser", conn)
                    .With("@AdminId", adminId)
                    .Build())
                    cmd.ExecuteNonQuery();
            });
        }

        // ── Helpers ───────────────────────────────────────────────────

        private static byte[] GenerateSalt()
        {
            var salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(salt);
            return salt;
        }

        private static byte[] HashPassword(string password, byte[] salt)
        {
            var passBytes = Encoding.UTF8.GetBytes(password);
            var combined = new byte[passBytes.Length + salt.Length];
            Buffer.BlockCopy(passBytes, 0, combined, 0, passBytes.Length);
            Buffer.BlockCopy(salt, 0, combined, passBytes.Length, salt.Length);
            using (var sha = SHA256.Create())
                return sha.ComputeHash(combined);
        }
    }
}
