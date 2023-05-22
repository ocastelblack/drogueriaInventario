using System;
using System.Collections.Generic;

namespace drogueriaInventario.Models;

public partial class Venta
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    public DateTime FechaVenta { get; set; }

    public virtual Producto Producto { get; set; } = null!;
}
