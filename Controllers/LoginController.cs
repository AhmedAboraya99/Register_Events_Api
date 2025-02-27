using login.DTOs;
using login.Repo.Login_Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly Ilogin _repo;
        private readonly IConfiguration _configuration;

        public LoginController(Ilogin repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginAdd log)
        {
            if (log == null || string.IsNullOrEmpty(log.Email) || string.IsNullOrEmpty(log.Password))
            {
                return BadRequest(new { Status = false, Message = "Invalid login request" });
            }

            var user = _repo.LoginFunction(log.Email, log.Password);

            if (user == null)
            {
                return Unauthorized(new { Status = false, Message = "Invalid email or password" });
            }

            // Generate JWT token
            var token = GenerateJwtToken(user.Email, user.Role);

            return Ok(new
            {
                Status = true,
                Message = "Login successful",
                Token = token,
                User = new
                {
                    user.Id,
                    user.Email,
                    user.Role
                }
            });
        }

        private string GenerateJwtToken(string email, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
