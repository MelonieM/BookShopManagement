using System;

namespace BookShopManagement.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; }
        public string Publisher { get; set; }
        public int? PublishedYear { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public override string ToString()
        {
            return $"[{ISBN}] {Title} by {Author} - ${Price} (Stock: {StockQuantity})";
        }
    }
}
