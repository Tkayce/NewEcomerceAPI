using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using EcommerceNEWAPI.DTOs;

[Route("api/[controller]")]
[ApiController]
public class ResetPasswordController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ResetPasswordController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
    {
        // Find user with a valid reset token
        var user = await _context.AppCustomers.FirstOrDefaultAsync(u => u.ResetToken != null && u.ResetTokenExpiry > DateTime.UtcNow);

        if (user == null)
        {
            return BadRequest(new { message = "Invalid or expired reset request." });
        }

        // Validate passwords
        if (model.NewPassword != model.ConfirmPassword)
        {
            return BadRequest(new { message = "Passwords do not match." });
        }

        // Update password
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        user.ResetToken = null; // Remove token after reset
        user.ResetTokenExpiry = null;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Password reset successfully." });
    }
}
