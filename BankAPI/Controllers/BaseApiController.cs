using System;
using ATM.Shared.DTOs;
using BankAPI.DataAccess.Exceptions;
using BankAPI.DataAccess.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly Logger _logger = Logger.Instance;

        /// <summary>
        /// Ejecuta una acción y envuelve el resultado en ApiResult.
        /// Todos los endpoints devuelven siempre 200 OK con ApiResult —
        /// el ATM distingue éxito/error por result.Success, no por HTTP status.
        /// </summary>
        protected IActionResult ExecuteSafe<T>(Func<T> action)
        {
            try
            {
                var result = action();
                return Ok(ApiResult<T>.Ok(result));
            }
            catch (BankDatabaseException ex)
            {
                _logger.LogError("Error BD [" + ex.ErrorNumber + "]: " + ex.Message);
                return Ok(ApiResult<T>.Fail(ex.ErrorNumber, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, "Error inesperado en controller");
                return Ok(ApiResult<T>.Fail(0, "Error interno del servidor."));
            }
        }

        protected IActionResult ExecuteSafe(Action action)
        {
            return ExecuteSafe<object>(() => { action(); return null!; });
        }
    }
}
