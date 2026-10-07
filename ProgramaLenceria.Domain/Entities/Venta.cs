using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramaLenceria.Domain.Entities;

public class Venta
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;

    // Relación opcional con Cliente (si es null, se considera consumidor final de paso)
    public int? ClienteId { get; set; }
    public Cliente? Cliente { get; set; }

    public string MedioPago { get; set; } = "Efectivo"; // Efectivo, Transferencia, Débito, Crédito
    public decimal Total { get; set; }
    public string? Observaciones { get; set; }

    public List<DetalleVenta> Detalles { get; set; } = new();
}
