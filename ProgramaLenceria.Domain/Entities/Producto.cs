using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramaLenceria.Domain.Enums;

namespace ProgramaLenceria.Domain.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty; // Ej: Corpiños, Bombachas, Pijamas
        public string? CategoriaDetalle { get; set; } // Ej: Encaje, Algodón, Microfibra

        // 👇 NUEVOS CAMPOS DE TEMPORADA Y ESTADO
        public Temporada Temporada { get; set; } = Temporada.Atemporal;
        public bool EsActivo { get; set; } = true;

        // Relación con Proveedor (opcional)
        public int? ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }

        // Relación 1 a Muchos con sus variantes (Talles / Colores)
        public List<VarianteProducto> Variantes { get; set; } = new();
    }
}
