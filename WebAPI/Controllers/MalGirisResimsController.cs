using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MalGirisResimsController : ControllerBase
    {
        private IMalGirisResimService _malGirisResimService;
        public MalGirisResimsController(IMalGirisResimService malGirisResimService)
        {
            _malGirisResimService = malGirisResimService;
        }
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _malGirisResimService.GetList();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }


        [HttpGet("getbyid")]
        public IActionResult GetById(int malGirisResimId)
        {
            var result = _malGirisResimService.GetById(malGirisResimId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpPost("add")]
        public IActionResult Add(MalGirisResim malGirisResim)
        {
            var result = _malGirisResimService.Add(malGirisResim);
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(MalGirisResim malGirisResim)
        {
           var result= _malGirisResimService.Update(malGirisResim);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("delete")]
        public IActionResult Delete(MalGirisResim malGirisResim)
        {
   
           var result = _malGirisResimService.Delete(malGirisResim);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
