using Bogus;
using Microsoft.VisualBasic;
using NetFirebase.Api.Data;
using NetFirebase.Api.Models.Domain;

namespace NetFirebase.Api.Extensions;

public static class DataPruebaExtensions
{
    public  static async void AddDataProductos(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider;
        var DbContext = service.GetRequiredService<DatabaseContext>();

        if (!DbContext.Productos.Any())
        {
            var productCollection = new List<Producto>();
            var faker = new Faker();
            for (var i = 1; i < 1000; i++)
            {
                productCollection.Add( new Producto {
                    Nombre = faker.Commerce.ProductName(),
                    Descripcion = faker.Commerce.ProductDescription(),
                    Precio = faker.Random.Decimal(10, 500)
                });
            }
            await DbContext.Productos.AddRangeAsync(productCollection);
            await DbContext.SaveChangesAsync();
        }
    }
}