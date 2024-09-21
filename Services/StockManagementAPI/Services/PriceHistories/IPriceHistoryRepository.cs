using StockManagementAPI.Dtos.PriceHistoryDtos;

namespace StockManagementAPI.Services.PriceHistories
{
    public interface IPriceHistoryRepository
    {
        public Task<bool> AddPriceHistoryAsync(CreatePriceHistoryDto priceHistory);
    }
}
