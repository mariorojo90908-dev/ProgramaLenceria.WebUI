using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramaLenceria.Domain.Entities
{
    public class VarianteProducto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public string Sku { get; set; } = string.Empty;
        public string Talle { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        // PRECIOS Y COSTOS
        public decimal PrecioCosto { get; set; } // Costo por unidad
        public decimal Precio { get; set; }      // Precio de venta público

        public int StockActual { get; set; }
        public int StockMinimo { get; set; } = 2;

        public bool TieneStockSuficiente(int cantidad) => StockActual >= cantidad;

        public void DescontarStock(int cantidad)
        {
            if (!TieneStockSuficiente(cantidad))
                throw new InvalidOperationException("Stock insuficiente para realizar la venta.");

            StockActual -= cantidad;
        }

        public void IncrementarStock(int cantidad) => StockActual += cantidad;
    }
}
