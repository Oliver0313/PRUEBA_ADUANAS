using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PRUEBA.Data;
using PRUEBA.Models;
using System.Linq;

namespace PRUEBA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] Usuario user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("Datos inválidos");

            if (_context.Usuario.Any(u => u.Username == user.Username))
                return BadRequest("El usuario ya existe");

            _context.Usuario.Add(user);
            _context.SaveChanges();

            return Ok("Usuario creado");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] Usuario user)
        {
            var usuario = _context.Usuario
                .FirstOrDefault(u =>
                    u.Username == user.Username &&
                    u.Password == user.Password);

            if (usuario == null)
                return Unauthorized("Informacion incorrectas");

            var token = GenerarToken(usuario);

            return Ok(new { token });
        }

        private string GenerarToken(Usuario usuario)
        {
            var jwtSettings = _config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
