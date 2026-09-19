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
}