using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramaLenceria.Domain.Entities;

public class DetalleVenta
{
    public int Id { get; set; }

    public int VentaId { get; set; }
    public Venta? Venta { get; set; }

    public int VarianteProductoId { get; set; }
    public VarianteProducto? VarianteProducto { get; set; }

    public int Cantidad { get; set; }
    public decimal PrecioUnitarioVenta { get; set; } // Precio al que se vendió en ese momento
    public decimal Subtotal => Cantidad * PrecioUnitarioVenta;
}
