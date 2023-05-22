﻿using Microsoft.AspNetCore.Mvc;
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

            _dbcontext.Ventas.Add(venta);
            await _dbcontext.SaveChangesAsync();

            return Ok(new { mensaje = "Venta registrada exitosamente" });
        }
    }
}