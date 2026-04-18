using ATM.Shared.DTOs.Notifications;
using BankAPI.Business.Services;
using BankAPI.DataAccess.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Route("api/notifications")]
    public class NotificationController : BaseApiController
    {
        private readonly NotificationService _service;

        public NotificationController(IConfiguration config)
        {
            _service = new NotificationService(
                new NotificationRepository(), config);
        }

        // GET api/notifications/settings/{accountId}
        [HttpGet("settings/{accountId:int}")]
        public IActionResult GetSettings(int accountId)
        {
            return ExecuteSafe(() => _service.GetSettings(accountId));
        }

        // POST api/notifications/settings
        [HttpPost("settings")]
        public IActionResult SaveSettings([FromBody] NotificationSettingsRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() =>
            {
                _service.SaveSettings(request);
                return new { Message = "Configuración guardada correctamente." };
            });
        }
    }
}
