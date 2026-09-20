namespace Auth.Domain.Constantes;
public class Rol
{
    public int Id { get; }
    public string Nombre { get; }
    private Rol(int id, string nombre)
    {
        Id = id;
        Nombre = nombre;
    }
    public static Rol Administrador = new Rol(1, "Administrador");
    public static Rol Cliente = new Rol(2, "Cliente");

    public static Rol ObtenerPorId(int id)
    {
        var diccionario = new Dictionary<int, Rol>()
        {
            {1, Rol.Administrador},
            {2, Rol.Cliente}
        };
        if(!diccionario.ContainsKey(id)) throw new InvalidOperationException("Clave de Rol no encontrada");

        return diccionario[id];
    }
    public static IReadOnlyList<Rol> Todos { get; } = new[] { Administrador, Cliente };
}