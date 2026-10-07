using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProgramaLenceria.Application.Common.Interfaces;
using ProgramaLenceria.Domain.Entities;

namespace ProgramaLenceria.Infrastructure.Repositories;

public class VentaRepository(LenceriaDbContext context)
    : Repository<Venta>(context), IVentaRepository
{
    public async Task<IEnumerable<Venta>> ObtenerTodasConDetallesAsync()
    {
        return await _context.Set<Venta>()
            .AsNoTracking()
            .Include(v => v.Detalles)
                .ThenInclude(d => d.VarianteProducto)
                    .ThenInclude(vp => vp.Producto)
            .Include(v => v.Cliente)
            .OrderByDescending(v => v.Fecha)
            .ToListAsync();
    }
}
