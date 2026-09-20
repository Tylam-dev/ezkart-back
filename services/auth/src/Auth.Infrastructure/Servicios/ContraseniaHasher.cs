using Auth.Application.IServicios;

namespace Auth.Infrastructure.Servicios;

public class ContraseniaHasher : IContraseniaHasher
{
    public bool VerificarHash(string contraseniaHasheada, string contrasenia)
    {
        return BCrypt.Net.BCrypt.Verify(contrasenia, contraseniaHasheada);
    }
    public string GenerarHash(string contrasenia)
    {
        return BCrypt.Net.BCrypt.HashPassword(contrasenia);
    }
}