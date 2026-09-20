using System.Security.Cryptography;
using System.Text;
using Auth.Application;
using Auth.Application.IServicios;
using Auth.Application.Utilidades;

public class AutenticacionServicio : IAutenticacionServicio
{
    private readonly IRepositorioAutenticacion _repositorioAutenticacion;
    private readonly IContraseniaHasher _contraseniaHasher;
    private readonly IJwtTokenServicio _jwtTokenServicio;
    public AutenticacionServicio(
        IRepositorioAutenticacion autenticacionRepositorio,
        IContraseniaHasher contraseniaHasher,
        IJwtTokenServicio jwtTokenServicio
    )
    {
        _repositorioAutenticacion = autenticacionRepositorio;
        _contraseniaHasher = contraseniaHasher;
        _jwtTokenServicio = jwtTokenServicio;
    }
    public async Task<LoggedDTO> LoginAsync(LoginDTO loginDTO)
    {
        
        var usuario = await _repositorioAutenticacion.ObtenerUsuarioLogin(loginDTO.NombreUsuario);
        
        if(usuario.Valor == null && usuario.Exitoso) throw new CredencialesInvalidas();

        if(!usuario.Exitoso) throw usuario.Excepcion ?? new Exception("Servicio no disponible en este momento");

        if (!_contraseniaHasher.VerificarHash(usuario.Valor.Contrasenia, loginDTO.Contrasena)) throw new CredencialesInvalidas();

        var token = _jwtTokenServicio.GenerarAccessToken(usuario.Valor);
        var refresToken = _jwtTokenServicio.GenerarRefreshToken();

        if(!token.Exitoso) throw token.Excepcion ?? new Exception("Servicio no disponible en este momento");

        if(!refresToken.Exitoso) throw token.Excepcion ?? new Exception("Servicio no disponible en este momento");

        EliminarRefreshTokenCaducados();
        
        await _repositorioAutenticacion.GuardarRefresToken(refresToken.Valor, usuario.Valor.Id);

        var dto = new LoggedDTO()
        {
            token = token.Valor,
            refreshToken = refresToken.Valor 
        };
        return dto;
    }
    private async void EliminarRefreshTokenCaducados()
    {
        var momentoActual = DateTime.UtcNow;
        var hashesTokenCaducados = await _repositorioAutenticacion.ObtenerHashRefreshTokensCaducados(momentoActual);

        if(hashesTokenCaducados.Valor is not null && hashesTokenCaducados.Valor.Count() > 0) 
            await _repositorioAutenticacion.EliminarRefreshTokens(hashesTokenCaducados.Valor);
    }
    public async Task<bool> LogoutAsync(string hashRefreshToken)
    {
        var eliminados = await _repositorioAutenticacion.EliminarRefreshTokens(new List<string>(){hashRefreshToken});
        
        return eliminados.Exitoso;
    }
    public async Task<TokenAcceso> RefrescarToken(string refreshToken, Guid usuarioId)
    {
        var usuario = await _repositorioAutenticacion.ObtenerUsuarioId(usuarioId);

        var refreshTokenHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));

        var fechaExpiracionToken = await _repositorioAutenticacion.ObtenerFechaCaducidadRefreshToken(refreshTokenHash);

        if((fechaExpiracionToken.Exitoso && fechaExpiracionToken.Valor is null) ||
            (fechaExpiracionToken.Valor >= DateTime.Now) ||
            (usuario.Valor is null)) 
                throw new CredencialesInvalidas();
        
        var token = _jwtTokenServicio.GenerarAccessToken(usuario.Valor);

        if(!token.Exitoso) throw token.Excepcion ?? new Exception("Servicio no disponible en este momento");

        return token.Valor; 
    }
}