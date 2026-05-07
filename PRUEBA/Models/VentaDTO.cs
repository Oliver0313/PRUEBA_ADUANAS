using System.Collections.Generic;

namespace PRUEBA.Models
{
    public class VentaDTO
    {
        public string Cliente { get; set; } // 👈 ahora es string
        public List<DetalleVentaDTO>? Detalles { get; set; }
    }
}