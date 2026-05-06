using System;
using System.Collections.Generic;

namespace PRUEBA.Models;

public partial class Venta
{
    public int Id { get; set; }

    public string Fecha { get; set; } = null!;

    public string Cliente { get; set; } = null!;

    public string ListaProductos { get; set; } = null!;

    public string Total { get; set; } = null!;
}
