using Business.DTOs.File;
using Business.DTOs.Reports;
using Business.Enums;
using Business.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PYP_Pre_Assignment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        readonly IFileService _fileService;
        readonly IReportsService _reportsService;

        public SaleController(IFileService fileService, IReportsService reportsService)
        {
            _fileService = fileService;
            _reportsService = reportsService;
        }
        [HttpPost("upload-data")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            await _fileService.ExcelToDb(new FileUploadDto
            {
                File = file
            });
            return NoContent();
        }
        [HttpPost("send-report/{type}")]
        public async Task<IActionResult> SendReport([FromQuery] SendReportDto sendReport)
        {
            return Ok(await _reportsService.SendReport(sendReport));
           }
    }
}
