using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AktifIslemsController : ControllerBase
    {
        private IAktifIslemService _aktifIslemService;
        public AktifIslemsController(IAktifIslemService aktifIslemService)
        {
            _aktifIslemService = aktifIslemService;
        }
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _aktifIslemService.GetList();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpGet("getalldto")]
        public IActionResult GetListDto()
        {
            var result = _aktifIslemService.GetListDto();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int aktifIslemId)
        {
            var result = _aktifIslemService.GetById(aktifIslemId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpPost("add")]
        public IActionResult Add(AktifIslem aktifIslem)
        {
            var result = _aktifIslemService.Add(aktifIslem);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(AktifIslem aktifIslem)
        {
            var result = _aktifIslemService.Update(aktifIslem);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("delete")]
        public IActionResult Delete(AktifIslem aktifIslem)
        {
   
           var result = _aktifIslemService.Delete(aktifIslem);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
