using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IslemsController : ControllerBase
    {
        private IIslemService _islemService;
        public IslemsController(IIslemService islemService)
        {
            _islemService = islemService;
        }
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _islemService.GetList();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
    
        [HttpGet("getbyid")]
        public IActionResult GetById(int islemId)
        {
            var result = _islemService.GetById(islemId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpPost("add")]
        public IActionResult Add(Islem islem)
        {
            var result = _islemService.Add(islem);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(Islem islem)
        {
            var result = _islemService.Update(islem);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("delete")]
        public IActionResult Delete(Islem islem)
        {
   
           var result = _islemService.Delete(islem);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
