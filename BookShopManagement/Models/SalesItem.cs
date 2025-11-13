namespace BookShopManagement.Models
{
    public class SaleItem
    {
        public int SaleItemID { get; set; }
        public int SaleID { get; set; }
        public int BookID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public Book Book { get; set; }

        public void CalculateSubtotal()
        {
            Subtotal = Quantity * UnitPrice;
        }
    }
}