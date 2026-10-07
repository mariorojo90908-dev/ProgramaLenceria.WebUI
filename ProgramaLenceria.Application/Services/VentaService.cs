using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProgramaLenceria.Application.Common.Interfaces;
using ProgramaLenceria.Domain.Entities;

namespace ProgramaLenceria.Application.Services;

public class VentaService(
    IVentaRepository ventaRepository,
    IRepository<DetalleVenta> detalleRepository,
    IRepository<VarianteProducto> varianteRepository) : IVentaService
{
    private readonly IVentaRepository _ventaRepository = ventaRepository;
    private readonly IRepository<DetalleVenta> _detalleRepository = detalleRepository;
    private readonly IRepository<VarianteProducto> _varianteRepository = varianteRepository;

    public async Task<IEnumerable<Venta>> ObtenerTodasAsync()
    {
        return await _ventaRepository.ObtenerTodasConDetallesAsync();
    }

    public async Task RegistrarVentaAsync(Venta venta)
    {
        if (!venta.Detalles.Any())
            throw new InvalidOperationException("La venta debe tener al menos un producto.");

        foreach (var det in venta.Detalles)
        {
            var variante = await _varianteRepository.GetByIdAsync(det.VarianteProductoId);
            if (variante == null)
                throw new InvalidOperationException($"No se encontró la variante #{det.VarianteProductoId}.");

            if (variante.StockActual < det.Cantidad)
                throw new InvalidOperationException($"Stock insuficiente para {variante.Talle}/{variante.Color}. Stock disponible: {variante.StockActual}.");

            variante.DescontarStock(det.Cantidad);
            await _varianteRepository.UpdateAsync(variante);
        }

        venta.Total = venta.Detalles.Sum(d => d.Subtotal);
        await _ventaRepository.AddAsync(venta);
    }

    public async Task EliminarVentaAsync(int id)
    {
        var venta = await _ventaRepository.GetByIdAsync(id);
        if (venta != null)
        {
            var detalles = await _detalleRepository.FindAsync(d => d.VentaId == id);

            foreach (var d in detalles)
            {
                var variante = await _varianteRepository.GetByIdAsync(d.VarianteProductoId);
                if (variante != null)
                {
                    variante.IncrementarStock(d.Cantidad);
                    await _varianteRepository.UpdateAsync(variante);
                }
                await _detalleRepository.DeleteAsync(d.Id);
            }

            await _ventaRepository.DeleteAsync(id);
        }
    }
}
