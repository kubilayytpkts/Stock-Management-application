using StockManagementAPI.Dtos.StockMovementDtos;

namespace StockManagementAPI.Services.StockMovements
{
    public interface IStockMovementRepository
    {
        public Task<bool> CreateStockMovementAsync(CreateStockMovementDto StockMovement);
        public Task<bool> DeleteStockMovementAsync(int id);
        public Task<ResultStockMovementDto> GetStockMovementByIdAsync(int id);
        public Task<bool> UpdateStockMovementAsync(UpdateStockMovementDto StockMovement);
        public Task<IEnumerable<ResultStockMovementDto>> GetAllStockMovementsAsync();
    }
}
