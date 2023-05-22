using System;
using System.Collections.Generic;

namespace drogueriaInventario.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public int Cantidad { get; set; }

    public int MinimoPedido { get; set; }

    public decimal Precio { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
