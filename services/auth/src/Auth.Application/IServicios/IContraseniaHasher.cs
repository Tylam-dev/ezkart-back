namespace Auth.Application.IServicios;

public interface IContraseniaHasher
{
    bool VerificarHash(string contraseniaHasheada, string contrasenia);
    string GenerarHash(string contrasenia);
}