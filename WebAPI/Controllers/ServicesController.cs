using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private IServisService _servisService;
        public ServicesController(IServisService servisService)
        {
            _servisService = servisService;
        }
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _servisService.GetList();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpGet("getallDto")]
        public IActionResult GetListDto()
        {
            var result = _servisService.GetListDto();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int servisId)
        {
            var result = _servisService.GetById(servisId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpPost("add")]
        public IActionResult Add(Servis servis)
        {
            var result = _servisService.Add(servis);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(Servis servis)
        {
            var result = _servisService.Update(servis);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("delete")]
        public IActionResult Delete(Servis servis)
        {
            //var servisExist = _servisService.ServisExist(servis);
            //if (!servisExist.IsSuccess)
            //{
            //    return BadRequest(servisExist.Message);
            //}
           var result = _servisService.Delete(servis);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
