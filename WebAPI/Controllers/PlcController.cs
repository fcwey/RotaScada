using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using S7.Net;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Connect([FromQuery] string ipAddress = "192.168.0.1")
        {
            var result = await _plcService.ConnectPlcAsync(ipAddress);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("setpoint")]
        public async Task<IActionResult> WriteSetpoint([FromQuery] int dbNumber, [FromQuery] int startByte, [FromQuery] float value)
        {
            var result = await _plcService.WriteSetpointAsync(dbNumber, startByte, value);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("pulse-bit")]
        public async Task<IActionResult> PulseBit([FromQuery] int dbNumber, [FromQuery] int startByte, [FromQuery] int bitNumber, [FromQuery] int durationMs = 500)
        {
            var result = await _plcService.PulseBitAsync(DataType.DataBlock, dbNumber, startByte, bitNumber, durationMs);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}