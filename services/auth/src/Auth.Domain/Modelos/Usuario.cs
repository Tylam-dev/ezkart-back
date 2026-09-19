using Auth.Domain.ObjValor;

namespace Auth.Domain;

public class Usuario
{
    public Guid Id { get; set; }
    public Rol Rol { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public CorreoElectronico CorreoElectronico { get; set; } = null!;
    public string NombreUsuario { get; set; } = null!;
    public DateTime FechaCreacion { get; set; }
}
