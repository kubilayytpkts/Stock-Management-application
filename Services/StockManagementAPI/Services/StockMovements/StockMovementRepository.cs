using Dapper;
using StockManagementAPI.Context;
using StockManagementAPI.Dtos.StockMovementDtos;

namespace StockManagementAPI.Services.StockMovements
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly DapperContext _dapperContext;

        public StockMovementRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<bool> CreateStockMovementAsync(CreateStockMovementDto StockMovement)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "INSERT INTO StockMovements(ProductId,MovementType,Quantity,MovementDate) VALUES (@ProductId,@MovementType,@Quantity,@MovementDate)";
                var responseMessage = await connection.ExecuteAsync(sql,StockMovement);

                return responseMessage == 1;
            }
        }

        public async Task<bool> DeleteStockMovementAsync(int id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "DELETE FROM StockMovements WHERE StockMovementId = @Id";
                var responseMessage = await connection.ExecuteAsync(sql, new { Id = id });
                return responseMessage == 1;
            }
        }

        public async Task<IEnumerable<ResultStockMovementDto>> GetAllStockMovementsAsync()
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "SELECT * FROM StockMovements";
                var resultStockMovements = await connection.QueryAsync<ResultStockMovementDto>(sql);
                
                return resultStockMovements;
            }
        }

        public async Task<ResultStockMovementDto> GetStockMovementByIdAsync(int id)
        {
            using (var connection = _dapperContext.CreateConnection()) 
            {
                string sql = "SELECT * FROM StockMovements WHERE StockMovementId = @Id";
                var resultStockMovement = await connection.QueryFirstOrDefaultAsync<ResultStockMovementDto>(sql);
                
                return resultStockMovement;
            }
        }

        public async Task<bool> UpdateStockMovementAsync(UpdateStockMovementDto StockMovement)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "UPDATE StockMovements SET ProductId = @ProductId, MovementType=@MovementType, Quantity=@Quantity, MovementDate = @MovementDate";
                var responseMessage = await connection.ExecuteAsync(sql, StockMovement);

                return responseMessage == 1;
            }
        }
    }
}
