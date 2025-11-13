namespace BookShopManagement.Models
{
    public class LowStockBook
    {
        public int BookID { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
    }
}