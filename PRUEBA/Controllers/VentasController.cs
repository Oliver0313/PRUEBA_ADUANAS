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

        // 🔹 GET: api/ventas
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ventas = await _context.Ventas.ToListAsync();
            return Ok(ventas);
        }

        // 🔹 GET: api/ventas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);

            if (venta == null)
                return NotFound();

            return Ok(venta);
        }

        // 🔹 POST: api/ventas
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Venta venta)
        {
            venta.Fecha = DateTime.Now; // se asigna automáticamente

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return Ok(venta);
        }

        // 🔹 PUT: api/ventas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Venta venta)
        {
            if (id != venta.Id)
                return BadRequest();

            _context.Entry(venta).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(venta);
        }

        // 🔹 DELETE: api/ventas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);

            if (venta == null)
                return NotFound();

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
