using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRUEBA.Data;
using PRUEBA.Models;

namespace PRUEBA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Productos.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound("Producto no encontrado");
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Producto producto)
        {
            if (producto == null) return BadRequest();

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return Ok(producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Producto producto)
        {
            if (id != producto.Id) return BadRequest("ID no coincide");

            var existe = await _context.Productos.AnyAsync(x => x.Id == id);
            if (!existe) return NotFound("Producto no existe");

            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();

            return Ok(producto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return Ok("Eliminado");
        }
    }
}
