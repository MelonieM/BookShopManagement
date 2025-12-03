using System;

namespace BookShopManagement.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
<<<<<<< HEAD
        public string Name { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
=======
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
        public DateTime RegistrationDate { get; set; }

        public override string ToString()
        {
            return $"[ID: {CustomerID}] {Name} - {Contact} ({Email})";
        }
    }
}