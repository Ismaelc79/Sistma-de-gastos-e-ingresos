using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize]
        [HttpGet("/summary")]
        public async Task<IActionResult> Get(string categoryType, DateTime startDate, DateTime endDate)
        {
            // Extracts user ID from the JW token
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized(new { message = "No se encontró el ID del usuario en el token" });

            // Assigns UserId to request before send it to service
            var userId = Ulid.Parse(userIdClaim);

            return Ok(await reportService.GetCategorySummaryAsync(userId, categoryType, startDate, endDate));
        }
    }
}
