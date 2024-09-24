using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockManagementUI.Dtos.StockMovementsDtos;
using System.Text;

namespace StockManagementUI.Controllers
{
    public class StockMovementsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient client;

        public StockMovementsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            client = _httpClientFactory.CreateClient();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStockQuantity(CreateStockMovementDto stockMovement)
        {
            stockMovement.MovementDate = DateTime.Now;
            var serializeStockMovement = JsonConvert.SerializeObject(stockMovement);    
            var stringContent = new StringContent(serializeStockMovement,Encoding.UTF8,"application/json");

            var responseCreateStockMovement =await client.PostAsync($"https://localhost:7000/api/StockMovements", stringContent);
            if(responseCreateStockMovement.IsSuccessStatusCode)
            {
                return Redirect("/Product/Index");
            }
            return Json(new { success = false });
        }
    }
}
