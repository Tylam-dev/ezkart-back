namespace Compras.Infrastructure.Persistencia.Semillas;

internal sealed class SemilleroGeneral
{
    private readonly SemillaDescuentos _descuentos;
    public SemilleroGeneral(
        SemillaDescuentos descuentos
    )
    {
        _descuentos = descuentos;
    }
    public async Task EjecutarSemillas()
    {
        await _descuentos.Sembrar();
    }
}
