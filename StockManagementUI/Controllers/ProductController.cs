using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StockManagementUI.Dtos.CategoryDtos;
using StockManagementUI.Dtos.ProductDtos;
using StockManagementUI.Models;
using System.Text;

namespace StockManagementUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient client;
        public ProductController(IHttpClientFactory _httpClientFactory)
        {
            client = _httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryFilterId = null)
        {
            var products = new List<ResultProductWithCategoryDto>();
            ViewBag.Categories = await Get_AllCategoriesAsync();


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

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int productId)
        {
            var responseProduct =await client.GetAsync("https://localhost:7000/api/Products/"+productId);
            if (responseProduct.IsSuccessStatusCode)
            {
                var jsonProduct =await responseProduct.Content.ReadAsStringAsync();
                var deserializeProduct = JsonConvert.DeserializeObject<ResultProductWithCategoryDto>(jsonProduct);

                ViewBag.SelectListCategory =await Select_ListCategoryAsync(deserializeProduct.CategoryId);
                return View(deserializeProduct);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProduct)
        {
            var serializeProduct = JsonConvert.SerializeObject(updateProduct);
            var stringContent = new StringContent(serializeProduct, Encoding.UTF8, "application/json");

            var responseAddProduct =await client.PutAsync("https://localhost:7000/api/Products", stringContent);
            if(responseAddProduct.IsSuccessStatusCode)
            {
                return Redirect("/Product/Index");
            }
            return View();
        }
        //ALERTLER EKLENİLECEK !! 


        // HELPERS METHOD 
        private async Task<List<SelectListItem>> Select_ListCategoryAsync(int categoryId)
        {
            List<SelectListItem> selectListItems = (from x in await Get_AllCategoriesAsync()
                                                    select new SelectListItem
                                                    {
                                                        Value=x.CategoryId.ToString(),
                                                        Text = x.CategoryName,
                                                        Selected = x.CategoryId == categoryId
                                                    }).ToList();
            return selectListItems;
        }

        private async Task<List<ResultCategoryDto>> Get_AllCategoriesAsync()
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
