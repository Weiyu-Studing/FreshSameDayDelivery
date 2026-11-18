using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
            return BadRequest("Username taken");

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return Ok("Registered");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Wrong username or password");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("MY_SUPER_SECRET_KEY_1234567890123456");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { token = tokenHandler.WriteToken(token) });
    }
}

public class UserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public class LoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
