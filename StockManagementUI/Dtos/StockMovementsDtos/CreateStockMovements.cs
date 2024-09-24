namespace StockManagementUI.Dtos.StockMovementsDtos
{
    public class CreateStockMovementDto
    {
        public int ProductId { get; set; }
        public  int Quantity { get; set; }
        public string MovementType { get; set; }
        public DateTime MovementDate { get; set; }
    }
}
