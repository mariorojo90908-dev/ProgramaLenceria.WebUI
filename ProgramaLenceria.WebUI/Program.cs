using Microsoft.EntityFrameworkCore;
using ProgramaLenceria.Infrastructure;
using ProgramaLenceria.WebUI.Components;
using ProgramaLenceria.Application.Common.Interfaces;
using ProgramaLenceria.Infrastructure.Repositories;
using ProgramaLenceria.Infrastructure.Services;
using ProgramaLenceria.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Registración de Entity Framework Core con SQLite
builder.Services.AddDbContext<LenceriaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro del repositorio genérico
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Registro del repositorio específico y servicios de Productos
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();

// Registros de servicios para Proveedores y Clientes
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// Registros para Compras y Ventas
builder.Services.AddScoped<ICompraService, CompraService>();
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<ICompraRepository, CompraRepository>();

// Registro del servicio de Respaldo de Base de Datos
builder.Services.AddScoped<IBackupService, BackupService>();

var app = builder.Build();

// -----------------------------------------------------------------------
// POBLADO E INICIALIZACIÓN DE LA BASE DE DATOS
// -----------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<LenceriaDbContext>();
        // Ejecuta la inicialización de la base de datos
        DbInitializer.SeedAsync(context).GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al inicializar la base de datos: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
