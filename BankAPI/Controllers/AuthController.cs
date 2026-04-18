using ATM.Shared.DTOs.Auth;
using BankAPI.Business.Services;
using BankAPI.DataAccess.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly AuthService _authService;

        public AuthController()
        {
            _authService = new AuthService(new AccountRepository());
        }

        // GET api/auth/start/{accountId}
        [HttpGet("start/{accountId:int}")]
        public IActionResult Start(int accountId)
        {
            return ExecuteSafe(() => _authService.GetAccountForAuth(accountId));
        }

        // POST api/auth/verify-pin
        [HttpPost("verify-pin")]
        public IActionResult VerifyPin([FromBody] VerifyPinRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _authService.VerifyPin(request));
        }

        // POST api/auth/create-session
        [HttpPost("create-session")]
        public IActionResult CreateSession([FromBody] CreateSessionRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _authService.CreateSession(request));
        }

        // POST api/auth/close-session
        [HttpPost("close-session")]
        public IActionResult CloseSession([FromBody] CloseSessionRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _authService.CloseSession(request));
        }
    }
}
