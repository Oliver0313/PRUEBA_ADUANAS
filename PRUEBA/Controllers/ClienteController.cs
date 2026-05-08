using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PRUEBA.Data;
using PRUEBA.Models;

namespace PRUEBA.Controllers
{
    [Authorize] // 🔒 Protege TODO el controller
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // 🔥 PRUEBA JWT
        [HttpGet("debug")]
        public IActionResult Debug()
        {
            var user = User.Identity?.Name;

            return Ok(new
            {
                message = "Entraste correctamente",
                user = user
            });
        }

        // 🔥 ENDPOINT DE PRUEBA SIMPLE
        [HttpGet("seguro")]
        public IActionResult Seguro()
        {
            return Ok("SI VES ESTO, PASASTE LA SEGURIDAD");
        }

        // 🔍 GET: api/clientes
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return Ok(clientes);
        }

        // 🔍 GET: api/clientes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        // ➕ POST: api/clientes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return Ok(cliente);
        }

        // ✏️ PUT: api/clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
                return BadRequest("El ID no coincide");

            var existe = await _context.Clientes.AnyAsync(c => c.Id == id);

            if (!existe)
                return NotFound();

            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(cliente);
        }

        // 🗑 DELETE: api/clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return Ok("Cliente eliminado");
        }
    }
}
