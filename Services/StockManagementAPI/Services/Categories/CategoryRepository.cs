using Dapper;
using StockManagementAPI.Context;
using StockManagementAPI.Dtos.CategoryDtos;

namespace StockManagementAPI.Services.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DapperContext _dapperContext;

        public CategoryRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<bool> CreateCategoryAsync(CreateCategoryDto Category)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "INSERT INTO Categories(CategoryName) VALUES (@CategoryName)";
                var result = await connection.ExecuteAsync(sql,Category);
                
                return result == 1 ? true : false;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "DELETE FORM Categories WHERE CategoryId = @CategoryId ";
                var result = await connection.ExecuteAsync(sql,new {CategoryId = id});

                return result == 1 ? true : false;
            }
        }

        public async Task<IEnumerable<ResultCategoryDto>> GetAllCategorysAsync()
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "SELECT * FROM Categories";
                var resultCategories = await connection.QueryAsync<ResultCategoryDto>(sql);

                return resultCategories;
            }
        }

        public async Task<ResultCategoryDto> GetCategoryByIdAsync(int id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "SELECT * FROM Categories WHERE CategoryId = @CategoryId";
                var resultCategory = await connection.QueryFirstOrDefaultAsync<ResultCategoryDto>(sql, new {CategoryId = id});

                return resultCategory;
            }
        }

        public async Task<bool> UpdateCategoryAsync(UpdateCategoryDto Category)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var sql = "UPDATE Categories SET CategoryName = @CategoryName WHERE CategoryId = @CategoryId";
                var result = await connection.ExecuteAsync(sql, Category);

                return result == 1;
            }
        }
    }
}
