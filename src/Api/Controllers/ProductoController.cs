using Microsoft.AspNetCore.Mvc;
using NetFirebase.Api.Models.Domain;
using NetFirebase.Api.Services.Productos;

namespace NetFirebase.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductoController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductoController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllProductos()
    {
        var resultados = await _productoService.GetAllProductos();
        return Ok(resultados);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProductoById(int id)
    {
        var resultado = await _productoService.GetProductoById(id);
        return Ok(resultado);
    }

    [HttpGet("nombre/{nombre}")]
    public async Task<ActionResult> GetProductoByNombre(string nombre)
    {
        var resultado = await _productoService.GetProductosByNombre(nombre);
        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult> CreateProducto(
        [FromBody] Producto request
    )
    {
        await _productoService.CreateProducto(request);
        return Ok();
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateProducto(
        [FromBody] Producto request
    )
    {
        await _productoService.UpdateProducto(request);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProducto(int id)
    {
        await _productoService.DeleteProducto(id);
        return Ok();
    }
}