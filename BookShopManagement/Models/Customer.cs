using System;

namespace BookShopManagement.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }

        public override string ToString()
        {
            return $"[ID: {CustomerID}] {Name} - {Contact} ({Email})";
        }
    }
}