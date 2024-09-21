using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using StockManagementAPI.Dtos.CategoryDtos;
using StockManagementAPI.Dtos.ProductDtos;
using StockManagementAPI.Services.Product;

namespace StockManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto product)
        {
            var result = await _productRepository.CreateProductAsync(product);
            
            return result == true ? Ok() : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto productDto)
        {
            var result = await _productRepository.UpdateProductAsync(productDto);

            return result == true ? NoContent() :BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var result = await _productRepository.DeleteProductAsync(productId);

            return result == true ? NoContent() :BadRequest(result); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdProduct(int productId)
        {
            var result = await _productRepository.GetProductByIdAsync(productId);

            return result !=null ? Ok(result) : NotFound(); 
        }

        [HttpGet("GetProductsByCategoryId/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategoryId(int categoryId)
        {
            var resultProductList = await _productRepository.GetProductsByCategoryId(categoryId);
            return Ok(resultProductList);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var result =await _productRepository.GetAllProductsAsync();

            return result != null ? Ok(result) :BadRequest("İşlem başarısız");
        }

    }
}
