using ATM.Shared.DTOs.BackOffice;
using BankAPI.Business.Services;
using BankAPI.DataAccess.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Route("api/backoffice")]
    public class AccountManagementController : BaseApiController
    {
        private readonly AccountManagementService _service;
        private readonly AuthBackOfficeService _authService;

        public AccountManagementController()
        {
            var repo     = new AccountManagementRepository();
            _service     = new AccountManagementService(repo);
            _authService = new AuthBackOfficeService(repo);
        }

        // Admin auth

        // GET api/backoffice/admin/salt/{username}
        [HttpGet("admin/salt/{username}")]
        public IActionResult GetAdminSalt(string username)
        {
            return ExecuteSafe(() => _authService.GetSalt(username));
        }

        // POST api/backoffice/admin/login
        [HttpPost("admin/login")]
        public IActionResult AdminLogin([FromBody] AdminLoginRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _authService.Login(request));
        }

        // Simpsons API

        // GET api/backoffice/characters?search=homer
        [HttpGet("characters")]
        public IActionResult SearchCharacters([FromQuery] string search = "")
        {
            return ExecuteSafe(() => _service.SearchCharacters(search));
        }

        // ── Cuentas ───────────────────────────────────────────────────

        // POST api/backoffice/accounts/create
        [HttpPost("accounts/create")]
        public IActionResult CreateAccount([FromBody] CreateAccountRequest request,
                                           [FromQuery] int adminId)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _service.CreateAccount(request, adminId));
        }

        // POST api/backoffice/accounts/enroll
        [HttpPost("accounts/enroll")]
        public IActionResult EnrollBiometric([FromBody] EnrollBiometricRequest request,
                                              [FromQuery] int adminId)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _service.EnrollBiometric(request, adminId));
        }

        // GET api/backoffice/accounts
        [HttpGet("accounts")]
        public IActionResult GetAccounts()
        {
            return ExecuteSafe(() => _service.GetAccounts());
        }

        // PUT api/backoffice/accounts/status
        [HttpPut("accounts/status")]
        public IActionResult UpdateStatus([FromBody] UpdateAccountStatusRequest request,
                                          [FromQuery] int adminId)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _service.UpdateStatus(request, adminId));
        }

        // POST api/backoffice/logs
        [HttpPost("logs")]
        public IActionResult GetLogs([FromBody] AuditLogRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _service.GetLogs(request));
        }

        // POST api/backoffice/statistics
        [HttpPost("statistics")]
        public IActionResult GetStatistics([FromBody] StatisticsRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _service.GetStatistics(request));
        }
    }
}
