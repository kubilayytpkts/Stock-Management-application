using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockManagementAPI.Dtos.CategoryDtos;
using StockManagementAPI.Services.Categories;

namespace StockManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto category)
        {
            var responseMessage = await _categoryRepository.CreateCategoryAsync(category);
            
            return responseMessage == true ? Ok("Kategori oluşturma işlemi başarılı") : BadRequest(responseMessage);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto category)
        {
            var responseMessage = await _categoryRepository.UpdateCategoryAsync(category);

            return responseMessage == true ? NoContent() : BadRequest(responseMessage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var responseMessage = await _categoryRepository.DeleteCategoryAsync(id);

            return responseMessage == true ? NoContent() : BadRequest(responseMessage);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdCategory(int id)
        {
            var resultCategory = await _categoryRepository.GetCategoryByIdAsync(id);

            return resultCategory == null ? NotFound() : Ok(resultCategory);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var resultCategoryList = await _categoryRepository.GetAllCategorysAsync();

            return resultCategoryList !=null ? Ok(resultCategoryList) : BadRequest(resultCategoryList);
        }
    }
}
