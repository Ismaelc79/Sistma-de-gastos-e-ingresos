using Application.DTOs.Transactions;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TransactionsController : ControllerBase
    {


        private readonly ITransactionService transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Ulid userID)
        {
            var transacciones = await transactionService.GetTransactionsByUserIdAsync(userID);
            return Ok(transacciones);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateTransactionRequest transactionRequest)
        {
            try
            {
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
