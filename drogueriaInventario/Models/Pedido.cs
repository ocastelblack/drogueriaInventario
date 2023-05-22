using System;
using System.Collections.Generic;

namespace drogueriaInventario.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
