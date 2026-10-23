using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("batch-pdf/{batchId}")]
        public async Task<IActionResult> GetBatchPdf(int batchId)
        {
            var result = await _reportService.GenerateBatchPdfReportAsync(batchId);
            if (result.IsSuccess && result.Data != null)
            {
                return File(result.Data, "text/plain", $"Batch_Report_{batchId}.txt");
            }
            return BadRequest(result);
        }

        [HttpGet("export-csv")]
        public async Task<IActionResult> ExportCsv([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var result = await _reportService.ExportTrendToCsvAsync(start, end, Array.Empty<string>());
            if (result.IsSuccess && result.Data != null)
            {
                return File(result.Data, "text/csv", $"Trend_Export_{start:yyyyMMdd}_{end:yyyyMMdd}.csv");
            }
            return BadRequest(result);
        }
    }
}