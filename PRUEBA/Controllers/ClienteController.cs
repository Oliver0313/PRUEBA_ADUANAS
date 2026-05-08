using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRUEBA.Data;
using PRUEBA.Models;

namespace PRUEBA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Clientes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound("Cliente no encontrado");
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente)
        {
            if (cliente == null) return BadRequest();

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return Ok(cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest("ID no coincide");

            var existe = await _context.Clientes.AnyAsync(x => x.Id == id);
            if (!existe) return NotFound("Cliente no existe");

            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();

            return Ok(cliente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return Ok("Eliminado");
        }
    }
}
