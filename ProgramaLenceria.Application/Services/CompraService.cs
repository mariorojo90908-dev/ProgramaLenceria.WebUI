using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProgramaLenceria.Application.Common.Interfaces;
using ProgramaLenceria.Domain.Entities;

namespace ProgramaLenceria.Application.Services;

public class CompraService(
    ICompraRepository compraRepository,
    IRepository<DetalleCompra> detalleRepository,
    IRepository<VarianteProducto> varianteRepository) : ICompraService
{
    private readonly ICompraRepository _compraRepository = compraRepository;
    private readonly IRepository<DetalleCompra> _detalleRepository = detalleRepository;
    private readonly IRepository<VarianteProducto> _varianteRepository = varianteRepository;

    public async Task<IEnumerable<Compra>> ObtenerTodasAsync()
    {
        return await _compraRepository.ObtenerTodasConDetallesAsync();
    }

    public async Task RegistrarCompraAsync(Compra compra)
    {
        if (compra.ProveedorId <= 0)
            throw new ArgumentException("Debe seleccionar un proveedor válido.");

        if (!compra.Detalles.Any())
            throw new ArgumentException("La compra debe incluir al menos un ítem.");

        compra.Total = compra.Detalles.Sum(d => d.Subtotal);

        // Incrementar Stock y actualizar Costo por variante
        foreach (var detalle in compra.Detalles)
        {
            var variante = await _varianteRepository.GetByIdAsync(detalle.VarianteProductoId);
            if (variante != null)
            {
                variante.IncrementarStock(detalle.Cantidad);

                if (detalle.PrecioCostoUnitario > 0)
                {
                    variante.PrecioCosto = detalle.PrecioCostoUnitario;
                    variante.Precio = Math.Round(variante.PrecioCosto * 1.60m, 2);
                }

                await _varianteRepository.UpdateAsync(variante);
            }
        }

        await _compraRepository.AddAsync(compra);
    }

    public async Task EliminarCompraAsync(int id)
    {
        var compra = await _compraRepository.ObtenerPorIdConDetallesAsync(id);
        if (compra != null)
        {
            // 1. Revertir el stock sumado previamente
            foreach (var detalle in compra.Detalles)
            {
                var variante = await _varianteRepository.GetByIdAsync(detalle.VarianteProductoId);
                if (variante != null)
                {
                    variante.DescontarStock(detalle.Cantidad);
                    await _varianteRepository.UpdateAsync(variante);
                }

                await _detalleRepository.DeleteAsync(detalle.Id);
            }

            // 2. Eliminar el registro maestro de la compra
            await _compraRepository.DeleteAsync(id);
        }
    }
}
