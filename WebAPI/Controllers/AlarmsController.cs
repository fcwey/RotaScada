using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmsController : ControllerBase
    {
        private readonly IAlarmService _alarmService;

        public AlarmsController(IAlarmService alarmService)
        {
            _alarmService = alarmService;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveAlarms()
        {
            var result = await _alarmService.GetActiveAlarmsAsync();
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("acknowledge/{alarmId}")]
        public async Task<IActionResult> AcknowledgeAlarm(int alarmId, [FromQuery] int userId, [FromQuery] string? note)
        {
            var result = await _alarmService.AcknowledgeAlarmAsync(alarmId, userId, note);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetAlarmHistory([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] int? priority)
        {
            var result = await _alarmService.GetAlarmHistoryAsync(start, end, priority);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}