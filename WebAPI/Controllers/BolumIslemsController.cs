using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BolumIslemsController : ControllerBase
    {
        private IBolumIslemService _bolumIslemService;
        public BolumIslemsController(IBolumIslemService bolumIslemService)
        {
            _bolumIslemService = bolumIslemService;
        }
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _bolumIslemService.GetList();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpGet("getallDto")]
        public IActionResult GetListDto()
        {
            var result = _bolumIslemService.GetListBolumIslemDtos();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int bolumIslemId)
        {
            var result = _bolumIslemService.GetById(bolumIslemId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpGet("getallbybolumid")]
        public IActionResult GetAllByBolumId(int bolumId)
        {
            var result = _bolumIslemService.GetListByBolumId(bolumId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpPost("add")]
        public IActionResult Add(BolumIslem bolumIslem)
        {
            var result = _bolumIslemService.Add(bolumIslem);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(BolumIslem bolumIslem)
        {
            var result = _bolumIslemService.Update(bolumIslem);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("delete")]
        public IActionResult Delete(BolumIslem bolumIslem)
        {
            //var servisExist = _servisService.ServisExist(servis);
            //if (!servisExist.IsSuccess)
            //{
            //    return BadRequest(servisExist.Message);
            //}
           var result = _bolumIslemService.Delete(bolumIslem);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
