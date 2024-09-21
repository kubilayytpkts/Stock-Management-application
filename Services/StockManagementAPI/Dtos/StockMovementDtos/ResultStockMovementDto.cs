namespace StockManagementAPI.Dtos.StockMovementDtos
{
    public class ResultStockMovementDto
    {
        public int StockMovementId { get; set; }
        public int ProductId { get; set; }
        public string MovementType { get; set; } //Giriş Çıkış
        public int Quantity { get; set; }
        public DateTime MovementDate { get; set; }
    }
}
