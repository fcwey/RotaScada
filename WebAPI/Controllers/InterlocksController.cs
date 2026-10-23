using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterlocksController : ControllerBase
    {
        private readonly IInterlockService _interlockService;

        public InterlocksController(IInterlockService interlockService)
        {
            _interlockService = interlockService;
        }

        [HttpGet("mimic-state")]
        public async Task<IActionResult> GetMimicState()
        {
            var result = await _interlockService.GetMimicStateAsync();
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("validate-safety")]
        public async Task<IActionResult> ValidateSafety([FromQuery] string operationCode)
        {
            var result = await _interlockService.ValidateOperationSafetyAsync(operationCode);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}