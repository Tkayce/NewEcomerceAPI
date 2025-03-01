using EcommerceNEWAPI.DTOs;
using EcommerceNEWAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ForgotPasswordController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public ForgotPasswordController(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }


    [HttpPost]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
    {
        var user = await _context.AppCustomers.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null)
        {
            return BadRequest(new { message = "Email not found." });
        }

        // Generate a reset token internally (not sent in email)
        user.ResetToken = Guid.NewGuid().ToString();
        user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1); // Token valid for 1 hour

        await _context.SaveChangesAsync();

        // Send email with reset link (without token)
        string resetLink = $"{model.FrontendUrl}/reset-password";
        string emailBody = $"Click the link to reset your password: <a href='{resetLink}'>Reset Password</a>";

        _emailService.SendEmailAsync (user.Email, "Password Reset", emailBody);

        return Ok(new { message = "Reset password email sent successfully." });
    }
}
