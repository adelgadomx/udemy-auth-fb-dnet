using Microsoft.EntityFrameworkCore;
using NetFirebase.Api.Data;
using NetFirebase.Api.Models.Domain;

namespace NetFirebase.Api.Services.Productos;

public class ProductoService : IProductoService
{
    private readonly DatabaseContext _context;

    public ProductoService( DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Producto>> GetAllProductos()
    {
        return await _context.Database.SqlQuery<Producto>(@$"
            SELECT * FROM Productos
        ").ToListAsync();
    }

    public async Task<Producto> GetProductoById(int id)
    {
        var resultado = await _context.Database.SqlQuery<Producto>(@$"
            SELECT * FROM Productos WHERE Id = {id}
        ").FirstOrDefaultAsync();

        return resultado is null? null! : resultado!;
    }

    public async Task CreateProducto(Producto producto)
    {
        var result = await _context.Database.ExecuteSqlAsync(@$"
          INSERT INTO Productos (
            Nombre, Descripcion, Precio
          ) VALUES (
            {producto.Nombre},
            {producto.Descripcion},
            {producto.Precio}
          )
        ");
        if (result <= 0) {
            throw new Exception("Errores al insertar producto");
        }
    }

    public async Task UpdateProducto(Producto producto)
    {
        var result = await _context.Database.ExecuteSqlAsync(@$"
          UPDATE Productos
             SET Nombre = {producto.Nombre}
               , Descripcion = {producto.Descripcion}
               , Precio = {producto.Precio}
           WHERE id = {producto.Id}
        ");
        if (result <= 0) {
            throw new Exception($"No se encontro el product {producto.Id}");
        }
    }

    public async Task DeleteProducto(int id)
    {
        var resultado = await _context.Database.ExecuteSqlAsync(@$"
            DELETE Productos WHERE Id = {id}
        ");

        if (resultado <= 0) {
            throw new Exception($"No se encontró el product {id}");
        }
    }

    public Task<bool> SaveChanges()
    {
        throw new NotImplementedException();
    }
}