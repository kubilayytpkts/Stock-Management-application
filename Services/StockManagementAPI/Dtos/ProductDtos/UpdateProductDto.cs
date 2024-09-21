﻿namespace StockManagementAPI.Dtos.ProductDtos
{
    public class UpdateProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public int MinimumStockLevel { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
    }
}