using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramaLenceria.Application.Common.Interfaces;
using ProgramaLenceria.Domain.Entities;

namespace ProgramaLenceria.Application.Services;

public class ProductoService(IProductoRepository productoRepository) : IProductoService
{
    private readonly IProductoRepository _productoRepository = productoRepository;

    public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
    {
        return await _productoRepository.GetAllAsync();
    }

    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        return await _productoRepository.GetByIdWithVariantesAsync(id);
    }

    public async Task CrearProductoAsync(Producto producto)
    {
        ValidarProducto(producto);
        await _productoRepository.AddAsync(producto);
    }

    public async Task ActualizarProductoAsync(Producto producto)
    {
        ValidarProducto(producto);
        await _productoRepository.UpdateAsync(producto);
    }

    public async Task EliminarProductoAsync(int id)
    {
        await _productoRepository.DeleteAsync(id);
    }

    private static void ValidarProducto(Producto producto)
    {
        if (string.IsNullOrWhiteSpace(producto.Nombre))
            throw new ArgumentException("El nombre del producto no puede estar vacío.");

        if (!producto.Variantes.Any())
            throw new ArgumentException("El producto debe contener al menos una variante (Talle/Color).");
    }
}
