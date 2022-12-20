using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusteriFiyatsController : ControllerBase
    {
        private IMusteriFiyatService  _musteriFiyatService;
        public MusteriFiyatsController(IMusteriFiyatService musteriFiyatService)
        {
            _musteriFiyatService = musteriFiyatService;
        }
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _musteriFiyatService.GetList();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
    
        [HttpGet("getbyid")]
        public IActionResult GetById(int musteriFiyatId)
        {
            var result = _musteriFiyatService.GetById(musteriFiyatId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpPost("add")]
        public IActionResult Add(MusteriFiyat musteriFiyat)
        {
            var result = _musteriFiyatService.Add(musteriFiyat);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(MusteriFiyat musteriFiyat)
        {
            var result = _musteriFiyatService.Update(musteriFiyat);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("delete")]
        public IActionResult Delete(MusteriFiyat musteriFiyat)
        {
   
           var result = _musteriFiyatService.Delete(musteriFiyat);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
