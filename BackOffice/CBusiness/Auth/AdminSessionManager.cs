using ATM.Shared.DTOs.BackOffice;
using BackOffice.Services.Implementations;

namespace Business.Auth
{
    /// <summary>
    /// Mantiene el estado del administrador autenticado en memoria.
    /// Singleton — un solo admin activo por instancia del back-office.
    /// </summary>
    public sealed class AdminSessionManager
    {
        private static readonly System.Lazy<AdminSessionManager> _instance =
            new System.Lazy<AdminSessionManager>(
                () => new AdminSessionManager());

        public static AdminSessionManager Instance
        { get { return _instance.Value; } }

        public AdminLoginResponse CurrentAdmin { get; private set; }
        public bool IsLoggedIn => CurrentAdmin != null;

        private AdminSessionManager() { }

        public void Open(AdminLoginResponse admin)
        {
            CurrentAdmin = admin;
            BackOfficeApiClient.SetToken(admin.Token);
        }

        public void Close()
        {
            CurrentAdmin = null;
            BackOfficeApiClient.ClearToken();
        }
        /// <summary>
        /// Verifica si el admin actual tiene permiso para abrir
        /// un formulario. El key es el nombre exacto del Form.
        /// </summary>
        public bool HasPermission(string permissionKey)
        {
            if (CurrentAdmin == null) return false;
            return CurrentAdmin.Permissions.Contains(permissionKey);
        }
    }
}
    

