namespace StockManagementAPI.Dtos.StockMovementDtos
{
    public class CreateStockMovementDto
    {
        public int ProductId { get; set; }
        public string MovementType { get; set; } //Giriş Çıkış
        public int Quantity { get; set; }
        public DateTime MovementDate { get; set; }
    }
}
