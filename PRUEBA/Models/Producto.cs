using System;
using System.Collections.Generic;

namespace PRUEBA.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public double Precio { get; set; }

    public int Stock { get; set; }
}
