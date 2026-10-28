using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Sadece giriş yapmış kullanıcılar istek atabilir
    public class SettingsController : ControllerBase
    {
        private readonly ISystemSettingService _settingService;

        public SettingsController(ISystemSettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpPost("update-parameter")]
        public async Task<IActionResult> UpdateParameter([FromBody] ParameterUpdateDto dto)
        {
            var result = await _settingService.UpdateCriticalParameterAsync(dto);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}