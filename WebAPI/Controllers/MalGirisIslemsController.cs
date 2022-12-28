using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MalGirisIslemsController : ControllerBase
    {
        private IMalGirisIslemService _malGirisIslemService;
        public MalGirisIslemsController(IMalGirisIslemService malGirisIslemService)
        {
            _malGirisIslemService = malGirisIslemService;
        }
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _malGirisIslemService.GetList();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }


        [HttpGet("getbyid")]
        public IActionResult GetById(int malGirisIslemId)
        {
            var result = _malGirisIslemService.GetById(malGirisIslemId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpPost("add")]
        public IActionResult Add(MalGirisIslem malGirisIslem)
        {
            var result = _malGirisIslemService.Add(malGirisIslem);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(MalGirisIslem malGirisIslem)
        {
           var result= _malGirisIslemService.Update(malGirisIslem);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("delete")]
        public IActionResult Delete(MalGirisIslem malGirisIslem)
        {
   
           var result = _malGirisIslemService.Delete(malGirisIslem);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
