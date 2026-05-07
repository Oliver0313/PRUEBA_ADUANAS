using System.Collections.Generic;

namespace PRUEBA.Models
{
    public class VentaDTO
    {
        public string Cliente { get; set; } 
        public List<DetalleVentaDTO>? Detalles { get; set; }
    }
}