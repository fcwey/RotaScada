//using Business.Abstract;
//using Entities.Concrete;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//namespace WebAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RecipesController : ControllerBase
//    {
//        private readonly IRecipeService _recipeService;

//        public RecipesController(IRecipeService recipeService)
//        {
//            _recipeService = recipeService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var result = await _recipeService.GetAllRecipesAsync();
//            if (result.IsSuccess) return Ok(result);
//            return BadRequest(result);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Add([FromBody] Recipe recipe)
//        {
//            var result = await _recipeService.AddRecipeAsync(recipe);
//            if (result.IsSuccess) return Ok(result);
//            return BadRequest(result);
//        }

//        [HttpPost("download/{recipeId}")]
//        public async Task<IActionResult> DownloadToPlc(int recipeId, [FromQuery] int targetDb = 2)
//        {
//            var result = await _recipeService.DownloadRecipeToPlcAsync(recipeId, targetDb);
//            if (result.IsSuccess) return Ok(result);
//            return BadRequest(result);
//        }
//    }
//}

using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        // Reçeteyi Sadece PLC DB'ye İndirir
        [HttpPost("download/{recipeId}")]
        public async Task<IActionResult> DownloadToPlc(int recipeId, [FromQuery] int targetDb = 2)
        {
            var result = await _recipeService.DownloadRecipeToPlcAsync(recipeId, targetDb);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        // Operatör "Reçeteyi Gönder & Başlat" Butonuna Bastığında Çalışır
        [HttpPost("download-and-start/{recipeId}")]
        public async Task<IActionResult> DownloadAndStart(int recipeId)
        {
            var result = await _recipeService.DownloadAndStartRecipeAsync(recipeId);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}