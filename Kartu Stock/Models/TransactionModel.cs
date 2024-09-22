namespace Kartu_Stock.Models
{
    public class TransactionModel
    {
        public int TransactionID { get; set; }
        public DateTime Date { get; set; }
        public string? No { get; set; }
        public string? Description { get; set; }
        public int PurchaseQuantity { get; set; }
        public int SalesQuantity { get; set; }
        public int Balance { get; set; }
    }
}
