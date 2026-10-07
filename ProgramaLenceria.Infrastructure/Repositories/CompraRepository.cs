using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProgramaLenceria.Application.Common.Interfaces;
using ProgramaLenceria.Domain.Entities;
using ProgramaLenceria.Infrastructure;

namespace ProgramaLenceria.Infrastructure.Repositories;

public class CompraRepository : Repository<Compra>, ICompraRepository
{
    private readonly LenceriaDbContext _context;

    public CompraRepository(LenceriaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Compra>> ObtenerTodasConDetallesAsync()
    {
        return await _context.Compras
            .Include(c => c.Proveedor)
            .Include(c => c.Detalles)
                .ThenInclude(d => d.VarianteProducto)
                    .ThenInclude(v => v.Producto)
            .OrderByDescending(c => c.Fecha)
            .ToListAsync();
    }

    public async Task<Compra?> ObtenerPorIdConDetallesAsync(int id)
    {
        return await _context.Compras
            .Include(c => c.Proveedor)
            .Include(c => c.Detalles)
                .ThenInclude(d => d.VarianteProducto)
                    .ThenInclude(v => v.Producto)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}