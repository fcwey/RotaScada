using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MalzemesController : ControllerBase
    {
        private IMalzemeService _malzemeService;
        public MalzemesController(IMalzemeService malzemeService)
        {
            _malzemeService = malzemeService;
        }
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _malzemeService.GetList();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpGet("getallDto")]
        public IActionResult GetListDto()
        {
            var result = _malzemeService.GetListMalzemeDtos();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int malzemeId)
        {
            var result = _malzemeService.GetById(malzemeId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpPost("add")]
        public IActionResult Add(Malzeme malzeme)
        {
            var result = _malzemeService.Add(malzeme);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(Malzeme malzeme)
        {
            var result = _malzemeService.Update(malzeme);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("delete")]
        public IActionResult Delete(Malzeme malzeme)
        {
           var result = _malzemeService.Delete(malzeme);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
