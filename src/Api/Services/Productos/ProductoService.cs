using Microsoft.EntityFrameworkCore;
using NetFirebase.Api.Data;
using NetFirebase.Api.Models.Domain;

namespace NetFirebase.Api.Services.Productos;

public class ProductoService : IProductoService
{
    private readonly DatabaseContext _context;

    public ProductoService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Producto>> GetAllProductos()
    {
        return await _context.Database.SqlQuery<Producto>(@$"
            SELECT * FROM fx_query_productos()
        ").ToListAsync();
    }

    public async Task<Producto> GetProductoById(int id)
    {
        var resultado = await _context.Database.SqlQuery<Producto>(@$"
            SELECT * FROM fx_query_productos({id}) ORDER BY ""Id""
        ").FirstOrDefaultAsync();

        return resultado is null ? null! : resultado!;
    }

    public async Task<List<Producto>> GetProductosByNombre(string nombre)
    {
        return await _context.Database.SqlQuery<Producto>(@$"
            SELECT * FROM fx_query_productos_by_nombre({nombre})
        ").ToListAsync();
    }

    public async Task CreateProducto(Producto producto)
    {
        try
        {
            await _context.Database.ExecuteSqlAsync(@$"
            CALL sp_insert_producto({producto.Precio}, {producto.Nombre}, {producto.Descripcion})
            ");

        }
        catch (System.Exception e)
        {
            throw new Exception($"Error al intentar insertar registro {producto.Nombre} \n {e}");
        }
    }

    public async Task UpdateProducto(Producto producto)
    {

        try
        {
            await _context.Database.ExecuteSqlAsync(@$"
            CALL sp_update_producto({producto.Id}, {producto.Precio}, {producto.Nombre}, {producto.Descripcion})
            ");
        }
        catch (System.Exception e)
        {
            throw new Exception($"Error al intentar actualizar registro {producto.Id} \n {e}");
        }

    }

    public async Task DeleteProducto(int id)
    {
        try
        {
            await _context.Database.ExecuteSqlAsync(@$"
            CALL sp_delete_producto({id})
            ");
        }
        catch (System.Exception e)
        {
            throw new Exception($"Error al intentar borrar registro {id} \n {e}");
        }

    }

    public Task<bool> SaveChanges()
    {
        throw new NotImplementedException();
    }
}
