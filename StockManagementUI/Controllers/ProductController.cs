using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StockManagementUI.Dtos.CategoryDtos;
using StockManagementUI.Dtos.ProductDtos;
using StockManagementUI.Models;

namespace StockManagementUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient client;
        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            client = _httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryFilterId = null)
        {
            var products = new List<ResultProductWithCategoryDto>();
            ViewBag.Categories = await GetAllCategoriesAsync();


            try
            {
                if (categoryFilterId != null)
                {
                    var filteredProductsResponse = await client.GetAsync($"https://localhost:7000/api/Products/GetProductsByCategoryId/{categoryFilterId}");
                    if (filteredProductsResponse.IsSuccessStatusCode)
                    {
                        var jsonFilteredProducts = await filteredProductsResponse.Content.ReadAsStringAsync();
                        products = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonFilteredProducts);
                    }
                }
                else
                {
                    var productsResponse = await client.GetAsync("https://localhost:7000/api/Products/GetProductsWithCategory");
                    if (productsResponse.IsSuccessStatusCode)
                    {
                        var jsonProducts = await productsResponse.Content.ReadAsStringAsync();
                        products = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonProducts);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return View(products);
        }

        private async Task<List<ResultCategoryDto>> GetAllCategoriesAsync()
        {
            var response = await client.GetAsync("https://localhost:7000/api/Categories");
            if (response.IsSuccessStatusCode)
            {
                var jsonCategories = await response.Content.ReadAsStringAsync();
                var deserializeCategories = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonCategories);

                return deserializeCategories;
            }
            return null;
        }
    }
}
