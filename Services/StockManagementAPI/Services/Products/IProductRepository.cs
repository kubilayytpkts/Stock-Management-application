using StockManagementAPI.Dtos.ProductDtos;

using StockManagementAPI.Entities;

namespace StockManagementAPI.Services.Product
{
    public interface IProductRepository
    {
        public Task<bool> CreateProductAsync(CreateProductDto product);
        public Task<bool> DeleteProductAsync(int id);
        public Task<ResultProductDto> GetProductByIdAsync(int id);
        public Task<bool> UpdateProductAsync(UpdateProductDto product);
        public Task<IEnumerable<ResultProductDto>> GetAllProductsAsync();
        public Task<IEnumerable<ResultProductWithCategoryDto>> GetProductsByCategoryId(int id);
        public Task<IEnumerable<ResultProductWithCategoryDto>> GetProductsWithCategory();
    }
}
