using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusterisController : ControllerBase
    {
        private IMusteriService _musteriService;
        public MusterisController(IMusteriService musteriService)
        {
            _musteriService = musteriService;
        }
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _musteriService.GetList();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpGet("getalldto")]
        public IActionResult GetListDto()
        {
            var result = _musteriService.GetListDto();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int musteriId)
        {
            var result = _musteriService.GetById(musteriId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpPost("add")]
        public IActionResult Add(Musteri musteri)
        {
            var result = _musteriService.Add(musteri);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(Musteri musteri)
        {
            var result = _musteriService.Update(musteri);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("delete")]
        public IActionResult Delete(Musteri musteri)
        {
   
           var result = _musteriService.Delete(musteri);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
