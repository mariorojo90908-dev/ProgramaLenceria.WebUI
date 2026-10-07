using ProgramaLenceria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramaLenceria.Application.Common.Interfaces
{
    public interface ICompraService
    {
        Task<IEnumerable<Compra>> ObtenerTodasAsync();
        Task RegistrarCompraAsync(Compra compra);
        Task EliminarCompraAsync(int id);
    }
}
