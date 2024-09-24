using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockManagementAPI.Dtos.StockMovementDtos;
using StockManagementAPI.Services.StockMovements;

namespace StockManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockMovementsController : ControllerBase
    {
        private readonly IStockMovementRepository _stockMovementRepository;

        public StockMovementsController(IStockMovementRepository stockMovementRepository)
        {
            _stockMovementRepository = stockMovementRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddStockMovement(CreateStockMovementDto stockMovement)
        {
            if(stockMovement.MovementDate == null)
                stockMovement.MovementDate = DateTime.Now;

            var result = await _stockMovementRepository.CreateStockMovementAsync(stockMovement);
            return result == true ? Ok() : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStockMovement(UpdateStockMovementDto stockMovement)
        {
            var result = await _stockMovementRepository.UpdateStockMovementAsync(stockMovement);
            return result == true ? NoContent() : BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStockMovement(int id)
        {
            var result = await _stockMovementRepository.DeleteStockMovementAsync(id);
            return result == true ? NoContent() : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdStockMovement(int id)
        {
            var result = await _stockMovementRepository.GetStockMovementByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStockMovements()
        {
            var result = await _stockMovementRepository.GetAllStockMovementsAsync();
            return Ok(result);
        }
    }
}
