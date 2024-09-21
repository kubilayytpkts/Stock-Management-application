
using Dapper;
using StockManagementAPI.Context;
using StockManagementAPI.Dtos.PriceHistoryDtos;

namespace StockManagementAPI.Services.PriceHistories
{
    public class PriceHistoryRepository : IPriceHistoryRepository
    {
        private readonly DapperContext _dapperContext;

        public PriceHistoryRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<bool> AddPriceHistoryAsync(CreatePriceHistoryDto priceHistory)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "INSERT INTO PriceHistory(ProductId,OldPrice,NewPrice,ChangeDate,Cost) VALUES (@ProductId,@OldPrice,@NewPrice,@ChangeDate,@Cost)";
                var responseMessage = await connection.ExecuteAsync(sql,priceHistory);

                return responseMessage == 1;
            }
        }
    }
}
