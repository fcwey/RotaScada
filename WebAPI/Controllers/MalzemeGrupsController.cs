using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MalzemeGrupsController : ControllerBase
    {
        private IMalzemeGrupService _malzemeGrupService;
        public MalzemeGrupsController(IMalzemeGrupService malzemeGrupService)
        {
            _malzemeGrupService = malzemeGrupService;
        }
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _malzemeGrupService.GetList();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
      

        [HttpGet("getbyid")]
        public IActionResult GetById(int malzemeGrupId)
        {
            var result = _malzemeGrupService.GetById(malzemeGrupId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest();

        }
        [HttpPost("add")]
        public IActionResult Add(MalzemeGrup malzemeGrup)
        {
            var result = _malzemeGrupService.Add(malzemeGrup);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("update")]
        public IActionResult Update(MalzemeGrup malzemeGrup)
        {
            var result = _malzemeGrupService.Update(malzemeGrup);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("delete")]
        public IActionResult Delete(MalzemeGrup malzemeGrup)
        {
            //var servisExist = _servisService.ServisExist(servis);
            //if (!servisExist.IsSuccess)
            //{
            //    return BadRequest(servisExist.Message);
            //}
           var result = _malzemeGrupService.Delete(malzemeGrup);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
