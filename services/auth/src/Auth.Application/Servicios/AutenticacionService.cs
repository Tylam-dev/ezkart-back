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
        
        var token = _jwtTokenServicio.GenerarAccessToken(usuario.Valor);
        var refresToken = _jwtTokenServicio.GenerarRefreshToken();

        if(usuario.Valor == null && usuario.Exitoso) throw new CredencialesInvalidas();

        if(!usuario.Exitoso) throw usuario.Excepcion ?? new Exception("Servicio no disponible en este momento");
        
        if (!_contraseniaHasher.VerificarHash(usuario.Valor.Contrasenia, loginDTO.Contrasena)) throw new CredencialesInvalidas();

        if(!token.Exitoso) throw token.Excepcion ?? new Exception("Servicio no disponible en este momento");

        if(!refresToken.Exitoso) throw token.Excepcion ?? new Exception("Servicio no disponible en este momento");

        this.EliminarRefreshTokenCaducados();
        
        await _repositorioAutenticacion.GuardarRefresToken(refresToken.Valor);

        var dto = new LoggedDTO()
        {
            token = token.Valor,
            refreshToken = refresToken.Valor 
        };
        return dto;
    }
    private async void EliminarRefreshTokenCaducados()
    {
        var momentoActual = DateTime.Now;
        var hashesTokenCaducados = await _repositorioAutenticacion.ObtenerHashRefreshTokensCaducados(momentoActual);

        if(hashesTokenCaducados.Valor is not null && hashesTokenCaducados.Valor.Count() > 0) 
            await _repositorioAutenticacion.EliminarRefreshTokens(hashesTokenCaducados.Valor);
    }
    public async Task<bool> LogoutAsync(string hashRefreshToken)
    {
        var eliminados = await _repositorioAutenticacion.EliminarRefreshTokens(new List<string>(){hashRefreshToken});
        
        return eliminados.Exitoso;
    }
}