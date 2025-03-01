﻿namespace ECommerceAPI.Model
{
    public class AppCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }

        public string Role { get; set; } = "Customer";
    }
}
