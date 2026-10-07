using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProgramaLenceria.Application.Common.Interfaces;
using ProgramaLenceria.Infrastructure;

namespace ProgramaLenceria.Infrastructure.Services
{
    public class BackupService : IBackupService
    {
        private readonly LenceriaDbContext _context;
        private readonly IConfiguration _config;
        private static readonly HttpClient _httpClient = new HttpClient();

        public BackupService(LenceriaDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public byte[] GenerarCsvStock()
        {
            var productos = _context.Productos
                .Include(p => p.Variantes)
                .AsNoTracking()
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Id;Categoria;Nombre;Marca;Temporada;Talle;Color;CostoUnitario;PrecioVenta;StockActual");

            foreach (var prod in productos)
            {
                foreach (var v in prod.Variantes)
                {
                    sb.AppendLine($"{prod.Id};\"{prod.Categoria}\";\"{prod.Nombre}\";\"{prod.Marca}\";\"{prod.Temporada}\";\"{v.Talle}\";\"{v.Color}\";{v.PrecioCosto};{v.Precio};{v.StockActual}");
                }
            }

            return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
        }

        public byte[] GenerarCsvVentas()
        {
            var ventas = _context.Ventas
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.VarianteProducto)
                        .ThenInclude(va => va.Producto)
                .AsNoTracking()
                .OrderByDescending(v => v.Fecha)
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("VentaId;Fecha;MedioPago;Total;Prenda;Talle;Color;Cantidad;PrecioUnitario;Subtotal");

            foreach (var venta in ventas)
            {
                foreach (var d in venta.Detalles)
                {
                    var nombrePrenda = d.VarianteProducto?.Producto?.Nombre ?? "Prenda";
                    var talle = d.VarianteProducto?.Talle ?? "-";
                    var color = d.VarianteProducto?.Color ?? "-";

                    sb.AppendLine($"{venta.Id};{venta.Fecha:yyyy-MM-dd HH:mm};\"{venta.MedioPago}\";{venta.Total};\"{nombrePrenda}\";\"{talle}\";\"{color}\";{d.Cantidad};{d.PrecioUnitarioVenta};{d.Subtotal}");
                }
            }

            return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
        }

        public async Task EnviarCsvPorCorreoAsync()
        {
            var apiKey = _config["ResendSettings:ApiKey"];
            var destinatario = _config["ResendSettings:RecipientEmail"];

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("No se encontró la ApiKey de Resend en appsettings.json.");

            if (string.IsNullOrWhiteSpace(destinatario))
                throw new InvalidOperationException("No se configuró el correo destinatario en appsettings.json.");

            var stockBase64 = Convert.ToBase64String(GenerarCsvStock());
            var ventasBase64 = Convert.ToBase64String(GenerarCsvVentas());

            var payload = new
            {
                from = "Sistema Lencería <onboarding@resend.dev>",
                to = new[] { destinatario },
                subject = $"Reporte de Stock y Ventas - {DateTime.Now:dd/MM/yyyy HH:mm}",
                html = "<p>Adjunto encontrarás las planillas actualizadas de <strong>Stock</strong> y <strong>Ventas</strong> en formato CSV compatibles con Excel.</p>",
                attachments = new[]
                {
                    new
                    {
                        filename = $"Stock_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                        content = stockBase64
                    },
                    new
                    {
                        filename = $"Ventas_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                        content = ventasBase64
                    }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Fallo al enviar correo mediante Resend: {response.StatusCode} - {errorMsg}");
            }
        }
    }
}
