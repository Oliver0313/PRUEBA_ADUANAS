using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRUEBA.Data;
using PRUEBA.Models;

namespace PRUEBA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Ventas.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) return NotFound("Venta no encontrada");
            return Ok(venta);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Venta venta)
        {
            if (venta == null) return BadRequest();

            venta.Fecha = DateTime.Now;

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return Ok(venta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Venta venta)
        {
            if (id != venta.Id) return BadRequest("ID no coincide");

            var existe = await _context.Ventas.AnyAsync(x => x.Id == id);
            if (!existe) return NotFound("Venta no existe");

            _context.Ventas.Update(venta);
            await _context.SaveChangesAsync();

            return Ok(venta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) return NotFound();

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            return Ok("Eliminado");
        }
    }
}
