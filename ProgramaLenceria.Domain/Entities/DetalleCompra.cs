using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramaLenceria.Domain.Entities
{
    public class DetalleCompra
    {
        public int Id { get; set; }
        public int CompraId { get; set; }
        public Compra? Compra { get; set; }

        public int VarianteProductoId { get; set; }
        public VarianteProducto? VarianteProducto { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioCostoUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioCostoUnitario;
    }
}
