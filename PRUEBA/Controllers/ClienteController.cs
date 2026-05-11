using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRUEBA.Data;
using PRUEBA.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRUEBA.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public ClientesController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginCliente([FromBody] ClienteLoginDto login)
        {
            if (login == null)
                return BadRequest("Datos inválidos");

            var correo = login.Correo?.Trim().ToLower();
            var password = login.Password?.Trim();

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c =>
                    c.Correo.ToLower() == correo &&
                    c.Password == password);

            if (cliente == null)
                return Unauthorized("Credenciales incorrectas");

            var token = GenerarTokenCliente(cliente);

            return Ok(new
            {
                token,
                role = "Cliente",
                nombre = cliente.Nombre,
                correo = cliente.Correo
            });
        }

        private string GenerarTokenCliente(Cliente cliente)
        {
            var jwtSettings = _config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, cliente.Nombre),
                new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
                new Claim(ClaimTypes.Email, cliente.Correo),
                new Claim(ClaimTypes.Role, "Cliente")
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clientes = await _context.Clientes.ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado");

            return Ok(cliente);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente)
        {
            if (cliente == null)
                return BadRequest("Datos inválidos");

            _context.Clientes.Add(cliente);

            await _context.SaveChangesAsync();

            return Ok(cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Cliente cliente)
        {
            if (cliente == null)
                return BadRequest("Datos inválidos");

            if (id != cliente.Id)
                return BadRequest("El ID no coincide");

            var existe = await _context.Clientes.AnyAsync(c => c.Id == id);

            if (!existe)
                return NotFound("Cliente no encontrado");

            _context.Entry(cliente).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(cliente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound("Cliente no encontrado");

            _context.Clientes.Remove(cliente);

            await _context.SaveChangesAsync();

            return Ok("Cliente eliminado");
        }
    }
}