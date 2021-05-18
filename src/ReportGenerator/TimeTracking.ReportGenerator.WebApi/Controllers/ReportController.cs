using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.Common.Wrapper;
using TimeTracking.ReportGenerator.Bl.Abstract;
using TimeTracking.ReportGenerator.Models;

namespace TimeTracking.ReportGenerator.WebApi.Controllers
{
    /// <summary>
    /// Report controller
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/reports")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        /// <summary>
        /// ReportController constructor
        /// </summary>
        /// <param name="reportService"></param>
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Generates report by config
        /// </summary>
        /// <param name="reportConfiguration"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FileContentResult> GenerateReport([FromBody] ReportConfiguration reportConfiguration)
        {
            var response = await _reportService.GenerateReportAsync(reportConfiguration);
            return new FileContentResult(response.Data.FileBytes, response.Data.FileContentType)
            {
                FileDownloadName = response.Data.FileName,
            };
        }
    }
}