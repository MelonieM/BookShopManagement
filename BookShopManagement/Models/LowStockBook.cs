namespace BookShopManagement.Models
{
    public class LowStockBook
    {
        public int BookID { get; set; }
<<<<<<< HEAD
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
=======
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
    }
}