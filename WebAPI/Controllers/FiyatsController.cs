using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiyatsController : ControllerBase
    {
        private IFiyatService _fiyatService;
        public FiyatsController(IFiyatService fiyatService)
        {
            _fiyatService = fiyatService;
        }
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _fiyatService.GetList();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpGet("getalldto")]
        public IActionResult GetListDto()
        {
            var result = _fiyatService.GetListFiyatDtos();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int fiyatId)
        {
            var result = _fiyatService.GetById(fiyatId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpPost("add")]
        public IActionResult Add(Fiyat fiyat)
        {
            var result = _fiyatService.Add(fiyat);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(Fiyat fiyat)
        {
            var result = _fiyatService.Update(fiyat);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("delete")]
        public IActionResult Delete(Fiyat fiyat)
        {
   
           var result = _fiyatService.Delete(fiyat);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
