using Business.Auth;
using BackOffice.Services.Implementations;

namespace BackOffice
{
    public static class AppServices
    {
        public static BackOfficeApiClient ApiClient   { get; private set; }
        public static AdminAuthService AuthService { get; private set; }

        public static void Initialize()
        {
            ApiClient   = new BackOfficeApiClient();
            AuthService = new AdminAuthService(ApiClient);
        }
    }
}
