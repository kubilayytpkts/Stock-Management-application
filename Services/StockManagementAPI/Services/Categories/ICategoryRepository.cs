using StockManagementAPI.Dtos.CategoryDtos;

namespace StockManagementAPI.Services.Categories
{
    public interface ICategoryRepository
    {
        public Task<bool> CreateCategoryAsync(CreateCategoryDto Category);
        public Task<bool> DeleteCategoryAsync(int id);
        public Task<ResultCategoryDto> GetCategoryByIdAsync(int id);
        public Task<bool> UpdateCategoryAsync(UpdateCategoryDto Category);
        public Task<IEnumerable<ResultCategoryDto>> GetAllCategorysAsync();
    }
}
