using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProgramaLenceria.Application.Common.Interfaces;
using ProgramaLenceria.Domain.Entities;

namespace ProgramaLenceria.Infrastructure.Repositories;

public class ProductoRepository(LenceriaDbContext context)
    : Repository<Producto>(context), IProductoRepository
{
    public new async Task<IEnumerable<Producto>> GetAllAsync()
    {
        return await _context.Set<Producto>()
            .AsNoTracking()
            .Include(p => p.Variantes)
            .Include(p => p.Proveedor)
            .ToListAsync();
    }

    public async Task<Producto?> GetByIdWithVariantesAsync(int id)
    {
        return await _context.Set<Producto>()
            .AsNoTracking()
            .Include(p => p.Variantes)
            .Include(p => p.Proveedor)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Producto>> GetProductosConStockBajoAsync(int limiteStock)
    {
        return await _context.Set<Producto>()
            .AsNoTracking()
            .Include(p => p.Variantes)
            .Where(p => p.Variantes.Sum(v => v.StockActual) <= limiteStock)
            .ToListAsync();
    }

    // 💡 SOBRESCRIBIMOS UpdateAsync PARA SINCRONIZAR PRODUCTO Y VARIANTES SIN ERROR DE TRACKING
    public new async Task UpdateAsync(Producto entity)
    {
        var existente = await _context.Set<Producto>()
            .Include(p => p.Variantes)
            .FirstOrDefaultAsync(p => p.Id == entity.Id);

        if (existente == null) return;

        // 1. Actualizar propiedades del producto
        existente.Nombre = entity.Nombre;
        existente.Marca = entity.Marca;
        existente.Categoria = entity.Categoria;
        existente.CategoriaDetalle = entity.CategoriaDetalle;
        existente.Temporada = entity.Temporada;
        existente.EsActivo = entity.EsActivo;
        existente.ProveedorId = entity.ProveedorId;

        // 2. Eliminar variantes que ya no están
        var idsEnviados = entity.Variantes.Select(v => v.Id).ToList();
        var aEliminar = existente.Variantes.Where(v => !idsEnviados.Contains(v.Id)).ToList();
        foreach (var v in aEliminar)
        {
            _context.Set<VarianteProducto>().Remove(v);
        }

        // 3. Actualizar existentes o agregar nuevas
        foreach (var vMod in entity.Variantes)
        {
            var vExistente = existente.Variantes.FirstOrDefault(v => v.Id == vMod.Id && v.Id != 0);
            if (vExistente != null)
            {
                vExistente.Talle = vMod.Talle;
                vExistente.Color = vMod.Color;
                vExistente.PrecioCosto = vMod.PrecioCosto;
                vExistente.Precio = vMod.Precio;
                vExistente.StockActual = vMod.StockActual;
                vExistente.StockMinimo = vMod.StockMinimo;
                vExistente.Sku = vMod.Sku;
            }
            else
            {
                existente.Variantes.Add(new VarianteProducto
                {
                    ProductoId = existente.Id,
                    Talle = vMod.Talle,
                    Color = vMod.Color,
                    PrecioCosto = vMod.PrecioCosto,
                    Precio = vMod.Precio,
                    StockActual = vMod.StockActual,
                    StockMinimo = vMod.StockMinimo,
                    Sku = vMod.Sku
                });
            }
        }

        await _context.SaveChangesAsync();
    }
}
