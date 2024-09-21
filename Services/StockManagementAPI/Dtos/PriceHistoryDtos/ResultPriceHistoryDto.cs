namespace StockManagementAPI.Dtos.PriceHistoryDtos
{
    public class ResultPriceHistoryDto
    {
        public int PriceHistoryId { get; set; }
        public int ProductId { get; set; }
        public decimal Cost { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
