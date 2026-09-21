using Inventario.Domain.Dominio;
using Inventario.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventario.Infrastructure.Persistencia.Semillas;
internal class SemillaProductos : ISemilla
{
    private readonly InventarioDBContext _context;
    private readonly ILogger<SemillaProductos> _logger;
    public SemillaProductos(InventarioDBContext context, ILogger<SemillaProductos> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task Sembrar()
    {
        var deseados = new List<Producto>()
        {
            new Producto { Codigo = "PRD-001", Nombre = "Mouse inalámbrico", Precio = 14.99m, Existencia = 120 },
            new Producto { Codigo = "PRD-002", Nombre = "Teclado mecánico", Precio = 59.90m, Existencia = 45 },
            new Producto { Codigo = "PRD-003", Nombre = "Monitor 24 pulgadas Full HD", Precio = 149.00m, Existencia = 18 },
            new Producto { Codigo = "PRD-004", Nombre = "Audífonos con micrófono", Precio = 34.50m, Existencia = 60 },
            new Producto { Codigo = "PRD-005", Nombre = "Webcam HD 1080p", Precio = 42.00m, Existencia = 25 },
            new Producto { Codigo = "PRD-006", Nombre = "Cable HDMI 2 metros", Precio = 6.75m, Existencia = 300 },
            new Producto { Codigo = "PRD-007", Nombre = "Cable USB-C a USB-C 1 metro", Precio = 8.25m, Existencia = 250 },
            new Producto { Codigo = "PRD-008", Nombre = "Cargador de pared 20W", Precio = 15.40m, Existencia = 90 },
            new Producto { Codigo = "PRD-009", Nombre = "Batería portátil 10000 mAh", Precio = 24.99m, Existencia = 70 },
            new Producto { Codigo = "PRD-010", Nombre = "Memoria USB 64 GB", Precio = 9.80m, Existencia = 200 },
            new Producto { Codigo = "PRD-011", Nombre = "Disco duro externo 1 TB", Precio = 62.00m, Existencia = 30 },
            new Producto { Codigo = "PRD-012", Nombre = "Tarjeta microSD 128 GB", Precio = 16.90m, Existencia = 85 },
            new Producto { Codigo = "PRD-013", Nombre = "Parlante bluetooth", Precio = 27.50m, Existencia = 40 },
            new Producto { Codigo = "PRD-014", Nombre = "Soporte para laptop", Precio = 19.99m, Existencia = 55 },
            new Producto { Codigo = "PRD-015", Nombre = "Mochila para laptop 15 pulgadas", Precio = 32.00m, Existencia = 35 },
            new Producto { Codigo = "PRD-016", Nombre = "Lámpara de escritorio LED", Precio = 21.00m, Existencia = 48 },
            new Producto { Codigo = "PRD-017", Nombre = "Silla de oficina ergonómica", Precio = 129.00m, Existencia = 12 },
            new Producto { Codigo = "PRD-018", Nombre = "Escritorio plegable", Precio = 84.90m, Existencia = 9 },
            new Producto { Codigo = "PRD-019", Nombre = "Regleta con 6 tomas", Precio = 12.30m, Existencia = 110 },
            new Producto { Codigo = "PRD-020", Nombre = "Alfombrilla para mouse grande", Precio = 7.99m, Existencia = 150 },
            new Producto { Codigo = "PRD-021", Nombre = "Cuaderno universitario 100 hojas", Precio = 2.50m, Existencia = 500 },
            new Producto { Codigo = "PRD-022", Nombre = "Caja de bolígrafos azules x12", Precio = 4.20m, Existencia = 320 },
            new Producto { Codigo = "PRD-023", Nombre = "Resaltadores x6 colores", Precio = 3.80m, Existencia = 210 },
            new Producto { Codigo = "PRD-024", Nombre = "Agenda 2026 tapa dura", Precio = 11.00m, Existencia = 65 },
            new Producto { Codigo = "PRD-025", Nombre = "Botella térmica 750 ml", Precio = 18.75m, Existencia = 80 },
            new Producto { Codigo = "PRD-026", Nombre = "Termo para café 350 ml", Precio = 13.90m, Existencia = 95 },
            new Producto { Codigo = "PRD-027", Nombre = "Audífonos inalámbricos", Precio = 49.99m, Existencia = 3 },
            new Producto { Codigo = "PRD-028", Nombre = "Tablet 10 pulgadas 64 GB", Precio = 219.00m, Existencia = 6 },
            new Producto { Codigo = "PRD-029", Nombre = "Router WiFi doble banda", Precio = 54.00m, Existencia = 22 },
            new Producto { Codigo = "PRD-030", Nombre = "Cámara de seguridad interior", Precio = 38.60m, Existencia = 0 }
        };

        var codigos = deseados.Select(p => p.Codigo).ToList();
        var existentes = await _context.Producto
            .Where(p => codigos.Contains(p.Codigo))
            .Select(p => p.Codigo)
            .ToListAsync();

        foreach (var deseado in deseados)
        {
            if (existentes.Contains(deseado.Codigo)) continue;

            _context.Producto.Add(new Entidades.Producto
            {
                Id = Guid.NewGuid(),
                Codigo = deseado.Codigo,
                Nombre = deseado.Nombre,
                Precio = deseado.Precio,
                Existencia = deseado.Existencia,
                FechaCreacion = DateTime.UtcNow,
                Estado = (char)EstadoEnum.Activo
            });
            _logger.LogInformation("Producto {Codigo} creado", deseado.Codigo);
        }

        await _context.SaveChangesAsync();
    }
}
