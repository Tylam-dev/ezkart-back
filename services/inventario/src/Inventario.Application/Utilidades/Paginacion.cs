namespace Inventario.Application.Utilidades;

public class Paginacion<T>
{
    public IReadOnlyList<T> Items { get; set; } = [];
    
    public int Pagina { get; set; }
    public int TamanoPagina { get; set; }
    
    public int TotalItems { get; set; }
    public int TotalPaginas { get; set; }

    public bool TienePaginaAnterior => Pagina > 1;
    public bool TienePaginaSiguiente => Pagina < TotalPaginas;
}