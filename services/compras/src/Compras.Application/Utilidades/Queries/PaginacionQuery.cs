namespace Compras.Application.Utilidades.Queries;

public class PaginacionQuery<T>
{
    public int Pagina { get; set; } = 1;
    public int TamanoPagina { get; set; } = 10;

    public T? Filtro { get; set; }

    public int Saltos => (Pagina - 1) * TamanoPagina;
}
