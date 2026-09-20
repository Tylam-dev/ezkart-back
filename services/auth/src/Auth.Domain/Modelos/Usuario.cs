using Auth.Domain.ObjValor;
using Auth.Domain.Constantes;
namespace Auth.Domain;

public class Usuario
{
    public Guid Id { get; private set; }
    public Rol Rol { get; private set; } = null!;
    public string Nombre { get; private set; } = null!;
    public CorreoElectronico CorreoElectronico { get; private set; } = null!;
    public string NombreUsuario { get; private set; } = null!;
    public DateTime FechaCreacion { get; private set; }
    public string Contrasenia { get; private set; } = null!;

    public Usuario(
        Guid id, 
        Rol rol,
        string nombre,
        CorreoElectronico correoElectronico,
        string nombreUsuario,
        string contrasenia)
    {
        Id = id;
        Rol = rol;
        Nombre = nombre;
        CorreoElectronico = correoElectronico;
        NombreUsuario = nombreUsuario;
        Contrasenia = contrasenia;
        FechaCreacion = DateTime.UtcNow;
    }
}
