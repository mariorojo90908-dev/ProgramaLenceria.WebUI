using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramaLenceria.Domain.Entities;

namespace ProgramaLenceria.Application.Common.Interfaces;

public interface IProductoRepository : IRepository<Producto>
{
    Task<Producto?> GetByIdWithVariantesAsync(int id);
    Task<IEnumerable<Producto>> GetProductosConStockBajoAsync(int limiteStock);
}
