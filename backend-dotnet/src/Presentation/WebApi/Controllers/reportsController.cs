using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class reportsController : ControllerBase
    {

        private readonly IReportService reportService;

        public reportsController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet("/summary")]
        public async Task<IActionResult> Get(Ulid userId, string categoryType, DateTime startDate, DateTime endDate) {

            return Ok(await reportService.GetCategorySummaryAsync(userId, categoryType, startDate, endDate));


        }
    }
}
