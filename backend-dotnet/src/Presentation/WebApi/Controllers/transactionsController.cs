using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> Get(Ulid userID) {

            var transacciones = await transactionService.GetTransactionsByUserIdAsync(userID);
            return Ok(transacciones);
        }

        [HttpPost]
        public IActionResult Add()
        {

            return Created("","");
        }

    }
}
