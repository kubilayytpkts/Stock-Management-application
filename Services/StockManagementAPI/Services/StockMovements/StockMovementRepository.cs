using Dapper;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockManagementAPI.Context;
using StockManagementAPI.Dtos.ProductDtos;
using StockManagementAPI.Dtos.StockMovementDtos;
using System.Text.Json.Serialization;

namespace StockManagementAPI.Services.StockMovements
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly DapperContext _dapperContext;
        private readonly HttpClient client;

        //"Giris" ==> Add Stock Quantity parameter 
        //"Cikis" ==> Delete Stock Quantity parameter 

        public StockMovementRepository(DapperContext dapperContext, IHttpClientFactory _httpClientFactory)
        {
            _dapperContext = dapperContext;
            client = _httpClientFactory.CreateClient();

        }

        public async Task<bool> CreateStockMovementAsync(CreateStockMovementDto StockMovement)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "INSERT INTO StockMovements(ProductId,MovementType,Quantity,MovementDate) VALUES (@ProductId,@MovementType,@Quantity,@MovementDate)";
                var responseCrateStockMovement = await connection.ExecuteAsync(sql, StockMovement);

                bool ChangeProductQuantityStatus = await ChangeProductStockQuantity(StockMovement.ProductId, StockMovement.MovementType, StockMovement.Quantity);


                return responseCrateStockMovement == 1 && ChangeProductQuantityStatus==true;
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

        private async Task<bool> ChangeProductStockQuantity(int productId, string movementType, int quantity)
        {

            var responseGetByIdProduct = await client.GetAsync($"https://localhost:7000/api/Products/{productId}");
            if (responseGetByIdProduct.IsSuccessStatusCode)
            {
                var jsonProduct = await responseGetByIdProduct.Content.ReadAsStringAsync();
                var deserializeProduct = JsonConvert.DeserializeObject<ResultProductDto>(jsonProduct);

                if (movementType == "Giris")
                {
                    deserializeProduct.StockQuantity += quantity;
                }
                else if (movementType == "Cikis")
                {
                    if (deserializeProduct.StockQuantity == 0)
                        return false;

                    deserializeProduct.StockQuantity -= quantity;
                }

                var responseUpdateProduct = await client.PutAsJsonAsync("https://localhost:7000/api/Products", deserializeProduct);

                return responseGetByIdProduct.IsSuccessStatusCode ? true : false;
            }

            return false;
        }
    }
}
