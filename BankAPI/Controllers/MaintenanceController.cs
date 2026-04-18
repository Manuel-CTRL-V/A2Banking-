using ATM.Shared.DTOs.Maintenance;
using BankAPI.Business.Services;
using BankAPI.DataAccess.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Route("api/maintenance")]
    public class MaintenanceController : BaseApiController
    {
        private readonly MaintenanceService _service;

        public MaintenanceController()
        {
            _service = new MaintenanceService(new MaintenanceRepository());
        }

        // ── Roles ─────────────────────────────────────────────────────

        // GET api/maintenance/roles
        [HttpGet("roles")]
        public IActionResult GetRoles()
            => ExecuteSafe(() => _service.GetRoles());

        // POST api/maintenance/roles
        [HttpPost("roles")]
        public IActionResult CreateRole([FromBody] CreateRoleRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _service.CreateRole(request));
        }

        // PUT api/maintenance/roles
        [HttpPut("roles")]
        public IActionResult UpdateRole([FromBody] UpdateRoleRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _service.UpdateRole(request));
        }

        // DELETE api/maintenance/roles/{roleId}
        [HttpDelete("roles/{roleId:int}")]
        public IActionResult DeleteRole(int roleId)
            => ExecuteSafe(() => { _service.DeleteRole(roleId); return "OK"; });

        // ── AdminUsers ────────────────────────────────────────────────

        // GET api/maintenance/users?search=
        [HttpGet("users")]
        public IActionResult GetAdminUsers([FromQuery] string search = "")
            => ExecuteSafe(() => _service.GetAdminUsers(search));

        // POST api/maintenance/users
        [HttpPost("users")]
        public IActionResult CreateAdminUser([FromBody] CreateAdminUserRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _service.CreateAdminUser(request));
        }

        // PUT api/maintenance/users
        [HttpPut("users")]
        public IActionResult UpdateAdminUser([FromBody] UpdateAdminUserRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _service.UpdateAdminUser(request));
        }

        // DELETE api/maintenance/users/{adminId}
        [HttpDelete("users/{adminId:int}")]
        public IActionResult DeleteAdminUser(int adminId)
            => ExecuteSafe(() => { _service.DeleteAdminUser(adminId); return "OK"; });
    }
}
