using System;
using System.Collections.Generic;

namespace BookShopManagement.Models
{
    public class SalesReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalBooksSold { get; set; }
        public List<Sale> Sales { get; set; } = new List<Sale>();
    }
}