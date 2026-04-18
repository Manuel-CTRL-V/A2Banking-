using System;
using ATM.Shared.Enums;

namespace ATM.Shared.Models
{
    /// <summary>
    /// Empleado del banco que opera el back-office.
    /// Es una entidad completamente separada del cliente bancario
    /// (Character/Account) — ciclos de vida distintos, credenciales
    /// distintas, sin biométrico propio.
    ///
    /// El ATM nunca interactúa directamente con AdminUser.
    /// El BackOffice lo usa para login y auditoría.
    /// El BankAPI lo usa para autorizar operaciones de gestión.
    ///
    /// Nota: PasswordHash y PasswordSalt no están aquí —
    /// son datos del servidor (BankAPI) que el BackOffice
    /// nunca necesita ver como modelo. Solo los envía como
    /// bytes en el DTO de login.
    /// </summary>
    public class AdminUser
    {
        public int      AdminId   { get; set; }
        public string   Username  { get; set; }
        public UserRole Role      { get; set; }
        public bool     IsActive  { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsSupervisor    => Role == UserRole.Supervisor;
        public bool IsBranchOfficer => Role == UserRole.BranchOfficer;
    }
}
