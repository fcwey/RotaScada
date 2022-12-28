using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MalGirisMalzemesController : ControllerBase
    {
        private IMalGirisMalzemeService _malGirisMalzemeService;
        public MalGirisMalzemesController(IMalGirisMalzemeService malGirisMalzemeService)
        {
            _malGirisMalzemeService = malGirisMalzemeService;
        }
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _malGirisMalzemeService.GetList();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }


        [HttpGet("getbyid")]
        public IActionResult GetById(int malGirisMalzemeId)
        {
            var result = _malGirisMalzemeService.GetById(malGirisMalzemeId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpPost("add")]
        public IActionResult Add(MalGirisMalzeme malGirisMalzeme)
        {
            var result = _malGirisMalzemeService.Add(malGirisMalzeme);
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(MalGirisMalzeme malGirisMalzeme)
        {
           var result= _malGirisMalzemeService.Update(malGirisMalzeme);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("delete")]
        public IActionResult Delete(MalGirisMalzeme malGirisMalzeme)
        {
   
           var result = _malGirisMalzemeService.Delete(malGirisMalzeme);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
