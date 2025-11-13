using System;

namespace BookShopManagement.Models
{
    /// <summary>
    /// User model for authentication and authorization
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Admin, Manager, Cashier
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLogin { get; set; }

        // Helper properties for role checking
        public bool IsAdmin => Role == "Admin";
        public bool IsManager => Role == "Manager";
        public bool IsCashier => Role == "Cashier";

        public override string ToString()
        {
            return $"{FullName} ({Role})";
        }
    }

    /// <summary>
    /// Static class to manage current logged-in user session
    /// </summary>
    public static class CurrentUser
    {
        // Current logged-in user
        public static User? LoggedInUser { get; set; }

        // Check if user is logged in
        public static bool IsLoggedIn => LoggedInUser != null;

        /// <summary>
        /// Check if current user has required permission
        /// </summary>
        /// <param name="requiredRole">Required role (Admin, Manager, Cashier)</param>
        /// <returns>True if user has permission</returns>
        public static bool HasPermission(string requiredRole)
        {
            if (!IsLoggedIn) return false;

            // Admin has all permissions
            if (LoggedInUser!.IsAdmin) return true;

            // Manager can do everything except user management
            if (LoggedInUser.IsManager && requiredRole != "Admin") return true;

            // Cashier has limited permissions (only sales and basic operations)
            if (LoggedInUser.IsCashier && requiredRole == "Cashier") return true;

            return false;
        }

        /// <summary>
        /// Clear current user session (logout)
        /// </summary>
        public static void Logout()
        {
            LoggedInUser = null;
        }

        /// <summary>
        /// Get current user's display name
        /// </summary>
        public static string GetDisplayName()
        {
            return IsLoggedIn ? LoggedInUser!.FullName : "Not Logged In";
        }

        /// <summary>
        /// Get current user's role
        /// </summary>
        public static string GetRole()
        {
            return IsLoggedIn ? LoggedInUser!.Role : "None";
        }
    }
}