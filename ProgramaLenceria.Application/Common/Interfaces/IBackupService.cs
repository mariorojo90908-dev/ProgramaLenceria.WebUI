using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramaLenceria.Application.Common.Interfaces
{
    public interface IBackupService
    {
        Task EnviarCsvPorCorreoAsync();
        byte[] GenerarCsvStock();
        byte[] GenerarCsvVentas();
    }
}
