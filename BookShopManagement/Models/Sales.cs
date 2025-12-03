using System;
using System.Collections.Generic;

namespace BookShopManagement.Models
{
    public class Sale
    {
        public int SaleID { get; set; }
        public int? CustomerID { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal FinalAmount { get; set; }
<<<<<<< HEAD
        public string? PaymentMethod { get; set; }
=======
        public string PaymentMethod { get; set; }
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
        public List<SaleItem> Items { get; set; } = new List<SaleItem>();

        public void CalculateTotals()
        {
            TotalAmount = 0;
            foreach (var item in Items)
            {
                TotalAmount += item.Subtotal;
            }
            FinalAmount = TotalAmount - (TotalAmount * DiscountPercent / 100);
        }
    }
}