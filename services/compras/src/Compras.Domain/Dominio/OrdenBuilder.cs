using Compras.Domain.Enums;
using Compras.Domain.Exepcion;

namespace Compras.Domain.Dominio;

public class OrdenBuilder
{
    private readonly Guid _usuarioId;
    private readonly List<OrdenDetalle> _detalles = new List<OrdenDetalle>();
    private DescuentoTemporada? _descuentoTemporada;

    public decimal Subtotal => _detalles.Sum(d => d.Subtotal);

    public OrdenBuilder(Guid usuarioId)
    {
        _usuarioId = usuarioId;
    }

    public OrdenBuilder AgregarProducto(Guid productoId, string codigo, string nombre, decimal precioUnitario, int cantidad)
    {
        if (cantidad <= 0)
            throw new ExepcionDominio("La cantidad debe ser mayor que cero.");
        if (precioUnitario < 0)
            throw new ExepcionDominio("El precio no puede ser negativo.");

        var detalle = _detalles.FirstOrDefault(d => d.ProductoId == productoId);

        if (detalle is null)
        {
            _detalles.Add(new OrdenDetalle()
            {
                ProductoId = productoId,
                Codigo = codigo,
                Nombre = nombre,
                PrecioUnitario = precioUnitario,
                Cantidad = cantidad
            });
        }
        else
        {
            detalle.Cantidad += cantidad;
        }

        return this;
    }

    public OrdenBuilder AplicarDescuento(DescuentoTemporada descuentoTemporada)
    {
        if (descuentoTemporada.Porcentaje < 0 || descuentoTemporada.Porcentaje > 100)
            throw new ExepcionDominio("El porcentaje de descuento debe estar entre 0 y 100.");

        _descuentoTemporada = descuentoTemporada;

        return this;
    }

    public Orden Construir()
    {
        if (_detalles.Count == 0)
            throw new ExepcionDominio("La orden debe tener al menos un producto.");

        return new Orden()
        {
            Id = Guid.NewGuid(),
            UsuarioId = _usuarioId,
            FechaCreacion = DateTime.UtcNow,
            EstadoOrden = EstadoOrdenEnum.Confirmada,
            DescuentoTemporada = _descuentoTemporada,
            Detalles = _detalles.ToList()
        };
    }
}
