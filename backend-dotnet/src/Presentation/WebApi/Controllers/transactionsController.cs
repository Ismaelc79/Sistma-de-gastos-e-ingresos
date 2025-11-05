using Application.DTOs.Transactions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class transactionsController : ControllerBase
    {


        private readonly ITransactionService transactionService;

        public transactionsController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Extracts user ID from the JW token
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized(new { message = "No se encontró el ID del usuario en el token" });

            // Assigns UserId to request before send it to service
            var userID = Ulid.Parse(userIdClaim);

            var transacciones = await transactionService.GetTransactionsByUserIdAsync(userID);
            return Ok(transacciones);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateTransactionRequest transactionRequest)
        {
            try
            {
                // Extracts user ID from the JW token
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized(new { message = "No se encontró el ID del usuario en el token" });

                // Assigns UserId to request before send it to service
                transactionRequest.UserId = Ulid.Parse(userIdClaim);

                var transaction = await transactionService.CreateTransactionAsync(transactionRequest);
                return Created("", transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(401, new { message = ex.Message });
            }
        }

    }
}
