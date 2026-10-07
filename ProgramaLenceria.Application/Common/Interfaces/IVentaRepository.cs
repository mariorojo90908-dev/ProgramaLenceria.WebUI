using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramaLenceria.Domain.Entities;

namespace ProgramaLenceria.Application.Common.Interfaces;

public interface IVentaRepository : IRepository<Venta>
{
    Task<IEnumerable<Venta>> ObtenerTodasConDetallesAsync();
}
