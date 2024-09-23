using StockManagementAPI.Context;
using StockManagementAPI.Entities;
using Dapper;
using StockManagementAPI.Dtos.ProductDtos;
using StockManagementAPI.Dtos.PriceHistoryDtos;
using Microsoft.AspNetCore.Mvc;
using StockManagementAPI.Services.PriceHistories;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using StockManagementAPI.Dtos.CategoryDtos;

namespace StockManagementAPI.Services.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _dapperContext;
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public ProductRepository(DapperContext dapperContext, IPriceHistoryRepository priceHistoryRepository)
        {
            _dapperContext = dapperContext;
            _priceHistoryRepository = priceHistoryRepository;
        }

        public async Task<bool> CreateProductAsync(CreateProductDto product)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = @"INSERT INTO Products(ProductName,CategoryId,Price,StockQuantity,Cost,MinimumStockLevel) VALUES (@ProductName,@CategoryId,@Price,@StockQuantity,@Cost,@MinimumStockLevel)";
                var result = await connection.ExecuteAsync(sql, product);

                return result == 1;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "DELETE FROM Products WHERE ProductId=@Id";
                var result = await connection.ExecuteAsync(sql, new { Id = id });

                return result == 1;
            }
        }

        public async Task<IEnumerable<ResultProductDto>> GetAllProductsAsync()
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "SELECT * FROM Products";
                var resultProducts = await connection.QueryAsync<ResultProductDto>(sql);

                return resultProducts;
            }
        }

        public async Task<ResultProductDto> GetProductByIdAsync(int id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "SELECT * FROM Products WHERE ProductId = @Id ";
                var result = await connection.QueryFirstOrDefaultAsync<ResultProductDto>(sql, new { Id = id });

                return result;
            }
        }

        public async Task<IEnumerable<ResultProductWithCategoryDto>> GetProductsByCategoryId(int id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string selectCategory = "SELECT * FROM Categories WHERE CategoryId = @CategoryId ";
                var resultCategory =await connection.QueryFirstOrDefaultAsync<ResultCategoryDto>(selectCategory, new { CategoryId = id });

                string selectProducts = "SELECT * FROM Products WHERE CategoryId = @CategoryId";
                var resultProducts = await connection.QueryAsync<ResultProductWithCategoryDto>(selectProducts, new { CategoryId = id });

                foreach (var product in resultProducts)
                {
                    if(product.CategoryId == resultCategory.CategoryId)
                    {
                        product.Category = new CategoryModel();
                        product.Category.CateoryId = resultCategory.CategoryId;
                        product.Category.CategoryName= product.ProductName;
                    }
                }

                return resultProducts;
            }
        }

        public async Task<bool> UpdateProductAsync(UpdateProductDto product)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                //OLD PRİCE
                string getOldPriceSql = "SELECT Price FROM Products WHERE ProductId = @ProductId";
                decimal oldPrice = await connection.QuerySingleAsync<decimal>(getOldPriceSql, new { ProductId = product.ProductId });


                //UPDATE PRODUCT
                string sql = @"UPDATE Products 
                          SET ProductName = @ProductName,Cost=@Cost,MinimumStockLevel=@MinimumStockLevel,CategoryId=@CategoryId,Price=@Price,StockQuantity=@StockQuantity
                          WHERE ProductId=@ProductId";
                var result = await connection.ExecuteAsync(sql, product);

                if (oldPrice != product.Price)
                {
                    var responseMessage = await _priceHistoryRepository.AddPriceHistoryAsync(new CreatePriceHistoryDto
                    {
                        ChangeDate = DateTime.Now,
                        Cost = product.Cost,
                        NewPrice = product.Price,
                        OldPrice = oldPrice,
                        ProductId = product.ProductId
                    });

                    if (responseMessage == false)
                        return false;
                }

                return result >= 0;
            }
        }

        public async Task<IEnumerable<ResultProductWithCategoryDto>> GetProductsWithCategory()
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string selectCategories = "SELECT * FROM Categories";
                var resultCategories = await connection.QueryAsync<ResultCategoryDto>(selectCategories);

                string selectProducts = "SELECT * FROM Products";
                var resultProducts = await connection.QueryAsync<ResultProductWithCategoryDto>(selectProducts);

                foreach (var category in resultCategories)
                {
                    foreach (var product in resultProducts)
                    {
                        if (product.CategoryId != category.CategoryId)
                            continue;

                        product.Category = new CategoryModel();
                        product.Category.CateoryId = category.CategoryId;
                        product.Category.CategoryName = category.CategoryName;
                    }
                }
                return resultProducts;
            }
        }
    }
}
