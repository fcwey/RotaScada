using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MalGirisKayitsController : ControllerBase
    {
        private IMalGirisKayitService _malGirisKayitService;
        public MalGirisKayitsController(IMalGirisKayitService malGirisKayitService)
        {
            _malGirisKayitService = malGirisKayitService;
        }
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _malGirisKayitService.GetList();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }


        [HttpGet("getbyid")]
        public IActionResult GetById(int malGirisKayitId)
        {
            var result = _malGirisKayitService.GetById(malGirisKayitId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpPost("add")]
        public IActionResult Add(MalGirisKayit malGirisKayit)
        {
            var result = _malGirisKayitService.Add(malGirisKayit);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(MalGirisKayit malGirisKayit)
        {
           var result= _malGirisKayitService.Update(malGirisKayit);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("delete")]
        public IActionResult Delete(MalGirisKayit malGirisKayit)
        {
   
           var result = _malGirisKayitService.Delete(malGirisKayit);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
