using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramaLenceria.Domain.Entities;

namespace ProgramaLenceria.Application.Common.Interfaces;

public interface IVentaService
{
    Task<IEnumerable<Venta>> ObtenerTodasAsync();
    Task RegistrarVentaAsync(Venta venta);
    Task EliminarVentaAsync(int id); // Para anular una venta y reponer el stock
}
