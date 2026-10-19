using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlcController : ControllerBase
    {
        private readonly IPlcService _plcService;

        public PlcController(IPlcService plcService)
        {
            _plcService = plcService;
        }

        [HttpPost("connect")]
        public IActionResult Connect(string ipAddress)
        {
            var result = _plcService.ConnectPlc(ipAddress);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("setpoint")]
        public IActionResult SetPoint(int db, int startByte, float value)
        {
            var result = _plcService.WriteSetpoint(db, startByte, value);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("read-temperature")]
        public IActionResult ReadTemperature(int db, int startByte)
        {
            var result = _plcService.ReadTemperature(db, startByte);
            if (result.IsSuccess) return Ok(result.Data);
            return BadRequest(result.Message);
        }
    }
}