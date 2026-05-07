using System;
using System.Collections.Generic;

namespace PRUEBA.Models;

public partial class Venta
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string Cliente { get; set; } = null!;

    public string ListaProductos { get; set; } = null!;

    public decimal Total { get; set; }
}
