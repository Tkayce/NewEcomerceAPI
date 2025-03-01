﻿using ECommerceAPI.DTOs;
using ECommerceAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ECommerceAPI.Controller
{
    public class AppCustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AppCustomerController(ApplicationDbContext context)

        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AppCustomerDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            // Check if the email is already used
            var existingUser = await _context.AppCustomers.FirstOrDefaultAsync(c => c.Email == request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email already used for SignUp, kindly login or click Forgotten Password." });
            }

            // Hash the password before saving
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new AppCustomer
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = hashedPassword // Ensure this matches the column in your DB
            };

            _context.AppCustomers.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful! Please login." });
        }

        // User Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            var user = await _context.AppCustomers.FirstOrDefaultAsync(c => c.Email == request.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid email or password." });
            }

            // Ensure the password column exists in AppCustomer
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return BadRequest(new { message = "User password not found." });
            }

            // Verify the hashed password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest(new { message = "Invalid email or password." });
            }

            // Return user details (without password)
            return Ok(new
            {
                message = "Login successful",
                user = new
                {
                    id = user.Id,
                    name = user.Name,
                    email = user.Email
                }
            });
        }


        // Get Customer Profile
        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var customer = await _context.AppCustomers.FindAsync(id);
            if (customer == null)
                return NotFound("Customer not found.");

            return Ok(new
            {
                customer.Id,
                customer.Name,
                customer.Email
            });
        }

        // Update Customer Profile
        [HttpPut("profile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, AppCustomerDTO request)
        {
            var customer = await _context.AppCustomers.FindAsync(id);
            if (customer == null)
                return NotFound("Customer not found.");

            if (!string.IsNullOrEmpty(request.Name))
                customer.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Email))
            {
                if (await _context.AppCustomers.AnyAsync(c => c.Email == request.Email && c.Id != id))
                    return BadRequest("Email already in use.");

                customer.Email = request.Email;
            }

            await _context.SaveChangesAsync();
            return Ok("Profile updated successfully.");
        }

        // Delete Customer
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.AppCustomers.FindAsync(id);
            if (customer == null)
                return NotFound("Customer not found.");

            _context.AppCustomers.Remove(customer);
            await _context.SaveChangesAsync();
            return Ok("Customer deleted successfully.");
        }
    }
}


  