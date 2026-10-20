using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditTrailService _auditTrailService;

        public AuditLogsController(IAuditTrailService auditTrailService)
        {
            _auditTrailService = auditTrailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] int? userId, [FromQuery] string? actionType)
        {
            var result = await _auditTrailService.GetAuditLogsAsync(start, end, userId, actionType);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}
