namespace Inventario.Infrastructure.Persistencia.Semillas;

internal sealed class SemilleroGeneral
{
    private readonly SemillaProductos _productos;
    public SemilleroGeneral(
        SemillaProductos productos
    )
    {
        _productos = productos;
    }
    public async Task EjecutarSemillas()
    {
        await _productos.Sembrar();
    }
}
