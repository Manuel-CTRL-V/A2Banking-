using ATM.Shared.DTOs.BackOffice;
using BankAPI.DataAccess.Interfaces;

namespace BankAPI.Business.Services
{
    public class AuthBackOfficeService
    {
        private readonly IAccountManagementRepository _repo;

        public AuthBackOfficeService(IAccountManagementRepository repo)
        {
            _repo = repo;
        }

        public GetAdminSaltResponse GetSalt(string username)
        {
            return _repo.GetAdminSalt(username);
        }

        public AdminLoginResponse Login(AdminLoginRequest request)
        {
            return _repo.AdminLogin(request);
        }
    }
}
