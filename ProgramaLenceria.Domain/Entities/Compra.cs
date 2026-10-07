using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramaLenceria.Domain.Entities
{
    public class Compra
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }

        // 👇 CAMPOS DE COMPROBANTE FLEXIBLES / OPCIONALES
        public string TipoComprobante { get; set; } = "Sin Comprobante"; // "Sin Comprobante", "Remito", "Factura"
        public string? NumeroComprobante { get; set; }                  // Ej: "0001-00004523" (opcional)
        public string? Observaciones { get; set; }                     // Ej: "Pago en efectivo, retiro local"

        public List<DetalleCompra> Detalles { get; set; } = new();
    }
}
