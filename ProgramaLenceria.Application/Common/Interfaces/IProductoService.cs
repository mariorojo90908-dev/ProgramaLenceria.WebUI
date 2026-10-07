using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramaLenceria.Domain.Entities;

namespace ProgramaLenceria.Application.Common.Interfaces;

public interface IProductoService
{
    Task<IEnumerable<Producto>> ObtenerTodosAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task CrearProductoAsync(Producto producto);
    Task ActualizarProductoAsync(Producto producto);
    Task EliminarProductoAsync(int id);
}
