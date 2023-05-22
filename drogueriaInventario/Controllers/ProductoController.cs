using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using drogueriaInventario.Models;

namespace drogueriaInventario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : Controller
    {
        public readonly DrogueriaContext _dbcontext;

        public ProductoController(DrogueriaContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Producto>>> GetAllProductos()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                lista = _dbcontext.Productos.ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, Response = lista });
            }
            
        }

        [HttpGet]
        [Route("Obtener/{Id:int}")]
        public IActionResult Obtener(int Id)
        {
            Producto oProducto = _dbcontext.Productos.Find(Id);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                oProducto = _dbcontext.Productos.Where(p => p.Id == Id).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oProducto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = oProducto });


            }
        }

        [HttpPost]
        [Route("Guardar_Producto")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
            try
            {
                _dbcontext.Productos.Add(objeto);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            Producto oProducto = _dbcontext.Productos.Find(objeto.Id);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");

            }

            try
            {
                oProducto.Nombre = objeto.Nombre is null ? oProducto.Nombre : objeto.Nombre;
                oProducto.Tipo = objeto.Tipo is null ? oProducto.Tipo : objeto.Tipo;
                oProducto.MinimoPedido =  objeto.MinimoPedido;
                oProducto.Precio =  objeto.Precio;

                _dbcontext.Productos.Update(oProducto);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("vender")]
        public async Task<ActionResult> VenderProducto(int id, int cantidad)
        {
            Producto producto = await _dbcontext.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound(new { mensaje = "El producto no se encontró" });
            }

            if (producto.Cantidad < cantidad)
            {
                return BadRequest(new { mensaje = "No hay suficientes unidades disponibles del producto" });
            }

            decimal precioVenta = cantidad * producto.Precio;

            // Actualizar la cantidad del producto en la tabla de productos
            producto.Cantidad -= cantidad;
            _dbcontext.Productos.Update(producto);

            // Registrar la venta en la tabla de ventas
            Venta venta = new Venta
            {
                ProductoId = producto.Id,
                Cantidad = cantidad,
                Precio = precioVenta,
                FechaVenta = DateTime.Now
            };

            

            try
            {
                _dbcontext.Ventas.Add(venta);
                await _dbcontext.SaveChangesAsync();
                string mensajeAlerta = "Venta registrada exitosamente";

                if (producto.Cantidad <= producto.MinimoPedido)
                {
                    mensajeAlerta = $"{mensajeAlerta} Quedan pocas unidades del producto. ¡Realice un nuevo pedido!";
                    mensajeAlerta = $"{mensajeAlerta} Cantidad actual: {producto.Cantidad}";
                    return BadRequest(new { mensaje = mensajeAlerta });
                }

                return Ok(new { mensaje = mensajeAlerta });
            }
            catch (Exception ex)
            {
                 return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }

            
        }

        [HttpPost]
        [Route("HacerPedido")]
        public IActionResult HacerPedido(int id, int cantidad)
        {
            try
            {
                // Validar si el producto existe en la base de datos
                var producto = _dbcontext.Productos.FirstOrDefault(p => p.Id == id);
                if (producto == null)
                {
                    return NotFound(new { mensaje = "El producto no se encontró" });
                }

                // Validar si la cantidad solicitada es válida
                if (cantidad <= 0)
                {
                    return BadRequest(new { mensaje = "La cantidad del pedido debe ser mayor a cero" });
                }

                // Validar si la cantidad solicitada supera el mínimo de pedido
                if (cantidad < producto.MinimoPedido)
                {
                    return BadRequest(new { mensaje = "La cantidad del pedido no cumple con el mínimo requerido" });
                }

                // Registrar el pedido en la base de datos
                var pedido = new Pedido
                {
                    ProductoId = id,
                    Cantidad = cantidad,
                    Estado = "Pendiente"
                };

                _dbcontext.Pedidos.Add(pedido);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Pedido realizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditarPedido")]
        public IActionResult EditarPedido(int id, string estado, int cantidad)
        {
            try
            {
                // Buscar el pedido en la base de datos
                var pedido = _dbcontext.Pedidos.FirstOrDefault(p => p.Id == id);
                if (pedido == null)
                {
                    return NotFound(new { mensaje = "El pedido no se encontró" });
                }

                // Validar si el estado es "ingresado"
                if (estado == "ingresado")
                {
                    // Validar si la cantidad ingresada es válida
                    if (cantidad <= 0)
                    {
                        return BadRequest(new { mensaje = "La cantidad ingresada debe ser mayor a cero" });
                    }

                    // Obtener el producto relacionado al pedido
                    var producto = _dbcontext.Productos.FirstOrDefault(p => p.Id == pedido.ProductoId);
                    if (producto == null)
                    {
                        return NotFound(new { mensaje = "El producto relacionado al pedido no se encontró" });
                    }
                    // Sumar la cantidad ingresada al producto
                    producto.Cantidad += cantidad;
                    _dbcontext.Productos.Update(producto);
                }
                // Guardar los cambios en el pedido y en el producto
                pedido.Estado = estado;
                _dbcontext.Pedidos.Update(pedido);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Pedido actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("ProductoMasVendido")]
        public ActionResult ProductoMasVendido()
        {
            var productoMasVendido = _dbcontext.Ventas
                .GroupBy(v => v.ProductoId)
                .OrderByDescending(g => g.Sum(v => v.Cantidad))
                .Select(g => new
                {
                    ProductoId = g.Key,
                    TotalVentas = g.Sum(v => v.Cantidad)
                })
                .FirstOrDefault();

            if (productoMasVendido == null)
            {
                return NotFound(new { mensaje = "No hay productos vendidos" });
            }

            Producto producto = _dbcontext.Productos.Find(productoMasVendido.ProductoId);

            return Ok(new { mensaje = "Producto más vendido", producto.Nombre, TotalVentas = productoMasVendido.TotalVentas });
        }

        [HttpGet]
        [Route("ProductoMenosVendido")]
        public ActionResult ProductoMenosVendido()
        {
            var productoMenosVendido = _dbcontext.Ventas
                .GroupBy(v => v.ProductoId)
                .OrderBy(g => g.Sum(v => v.Cantidad))
                .Select(g => new
                {
                    ProductoId = g.Key,
                    TotalVentas = g.Sum(v => v.Cantidad)
                })
                .FirstOrDefault();

            return Ok(new { mensaje = "Venta registrada exitosamente" });
            {
                return NotFound(new { mensaje = "No hay productos vendidos" });
            }

            Producto producto = _dbcontext.Productos.Find(productoMenosVendido.ProductoId);

            return Ok(new { mensaje = "Producto menos vendido", producto.Nombre, TotalVentas = productoMenosVendido.TotalVentas });
        }

        [HttpGet]
        [Route("TotalDineroVentas")]
        public ActionResult TotalDineroVentas()
        {
            decimal totalDineroVentas = _dbcontext.Ventas.Sum(v => v.Precio);

            return Ok(new { mensaje = "Total de dinero obtenido por ventas", TotalDineroVentas = totalDineroVentas });
        }

        [HttpDelete]
        [Route("EliminarProducto/{id}")]
        public IActionResult EliminarProducto(int id)
        {
            try
            {
                // Buscar el producto en la base de datos
                var producto = _dbcontext.Productos.FirstOrDefault(p => p.Id == id);
                if (producto == null)
                {
                    return NotFound(new { mensaje = "El producto no se encontró" });
                }

                // Verificar si hay ventas relacionadas al producto
                var ventasRelacionadas = _dbcontext.Ventas.Any(v => v.ProductoId == id);
                // Verificar si hay pedidos relacionados al producto
                var pedidosRelacionados = _dbcontext.Pedidos.Any(p => p.ProductoId == id);

                // Verificar si el producto "Eliminado" ya existe en la base de datos
                var productoEliminado = _dbcontext.Productos.FirstOrDefault(p => p.Nombre == "Eliminado" && p.Tipo == "Eliminado");
                if (productoEliminado == null)
                {
                    // Crear un nuevo producto "Eliminado"
                    productoEliminado = new Producto
                    {
                        Nombre = "Eliminado",
                        Tipo = "Eliminado",
                        Cantidad = 0,
                        MinimoPedido = 0,
                        Precio = 0
                    };

                    _dbcontext.Productos.Add(productoEliminado);
                    _dbcontext.SaveChanges();
                }

                // Actualizar los registros de Ventas y Pedidos con el nuevo producto
                if (ventasRelacionadas)
                {
                    var ventas = _dbcontext.Ventas.Where(v => v.ProductoId == id).ToList();
                    foreach (var venta in ventas)
                    {
                        venta.ProductoId = productoEliminado.Id;
                    }
                }

                if (pedidosRelacionados)
                {
                    var pedidos = _dbcontext.Pedidos.Where(p => p.ProductoId == id).ToList();
                    foreach (var pedido in pedidos)
                    {
                        pedido.ProductoId = productoEliminado.Id;
                    }
                }

                _dbcontext.SaveChanges();

                // Eliminar el producto original
                _dbcontext.Productos.Remove(producto);
                _dbcontext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Producto eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


    }
}
