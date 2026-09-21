using Compras.Domain.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Compras.Infrastructure.Persistencia.Semillas;
internal class SemillaDescuentos : ISemilla
{
    private readonly ComprasDBContext _context;
    private readonly ILogger<SemillaDescuentos> _logger;
    public SemillaDescuentos(ComprasDBContext context, ILogger<SemillaDescuentos> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task Sembrar()
    {
        var deseados = new List<DescuentoTemporada>()
        {
            new DescuentoTemporada
            {
                Nombre = "Descuento de temporada",
                Porcentaje = 10m,
                FechaDesde = DateTime.UtcNow.Date,
                FechaHasta = DateTime.UtcNow.Date.AddYears(1)
            }
        };

        var nombres = deseados.Select(d => d.Nombre).ToList();
        var existentes = await _context.DescuentoTemporada
            .Where(d => nombres.Contains(d.Nombre))
            .Select(d => d.Nombre)
            .ToListAsync();

        foreach (var deseado in deseados)
        {
            if (existentes.Contains(deseado.Nombre)) continue;

            _context.DescuentoTemporada.Add(new Entidades.DescuentoTemporada
            {
                Id = Guid.NewGuid(),
                Nombre = deseado.Nombre,
                Porcentaje = deseado.Porcentaje,
                FechaDesde = deseado.FechaDesde,
                FechaHasta = deseado.FechaHasta,
                FechaCreacion = DateTime.UtcNow
            });
            _logger.LogInformation("Descuento {Nombre} creado", deseado.Nombre);
        }

        await _context.SaveChangesAsync();
    }
}
