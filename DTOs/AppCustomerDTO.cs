﻿using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class AppCustomerDTO
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
