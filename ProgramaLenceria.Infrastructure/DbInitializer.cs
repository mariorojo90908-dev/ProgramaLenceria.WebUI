using ProgramaLenceria.Domain.Entities;

namespace ProgramaLenceria.Infrastructure;

public static class DbInitializer
{
    public static async Task SeedAsync(LenceriaDbContext context)
    {
        // Asegura que la base de datos y sus tablas estén creadas
        await context.Database.EnsureCreatedAsync();
        // Si ya existen productos en la BD, no volvemos a insertar
        if (context.Productos.Any())
            return;

        // 1. Crear Proveedor de prueba
        var proveedor = new Proveedor
        {
            RazonSocial = "Textil Encajes S.A.",
            ContactoNombre = "María López",
            Telefono = "11-4444-5555"
        };

        // 2. Crear Productos con sus Variantes (Talle, Color, Precio y Stock)
        var productos = new List<Producto>
        {
            new Producto
            {
                Nombre = "Corpiño Encaje Bralette",
                Categoria = "Corpiños",
                CategoriaDetalle = "Encaje",
                Variantes = new List<VarianteProducto>
                {
                    new VarianteProducto { Sku = "COR-001-85-NEG", Talle = "85", Color = "Negro", Precio = 12500, StockActual = 10, StockMinimo = 2 },
                    new VarianteProducto { Sku = "COR-001-90-ROJ", Talle = "90", Color = "Rojo", Precio = 12500, StockActual = 1, StockMinimo = 2 },
                    new VarianteProducto { Sku = "COR-001-95-BLA", Talle = "95", Color = "Blanco", Precio = 12500, StockActual = 5, StockMinimo = 2 }
                }
            },
            new Producto
            {
                Nombre = "Bombacha Colaless Regulable",
                Categoria = "Bombachas",
                CategoriaDetalle = "Algodón",
                Variantes = new List<VarianteProducto>
                {
                    new VarianteProducto { Sku = "COL-002-U-NEG", Talle = "Único", Color = "Negro", Precio = 4500, StockActual = 15, StockMinimo = 3 },
                    new VarianteProducto { Sku = "COL-002-U-ROJ", Talle = "Único", Color = "Rojo", Precio = 4500, StockActual = 8, StockMinimo = 3 }
                }
            }
        };

        await context.Proveedores.AddAsync(proveedor);
        await context.Productos.AddRangeAsync(productos);
        await context.SaveChangesAsync();
    }
}
