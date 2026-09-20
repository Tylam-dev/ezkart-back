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

        var usuarioDTO = new UsuarioDTO()
        {
            Nombre = usuario.Valor.Nombre,
            NombreUsuario = usuario.Valor.NombreUsuario,
            CorreoElectronico = usuario.Valor.CorreoElectronico.Valor,
            Rol = usuario.Valor.Rol.Nombre
        };

        var dto = new LoggedDTO()
        {
            token = token.Valor,
            refreshToken = refresToken.Valor,
            Usuario = usuarioDTO 
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
    public async Task<LoggedDTO> RefrescarToken(string refreshToken)
    {
        var refreshTokenHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));

        var fechaExpiracionToken = await _repositorioAutenticacion.ObtenerFechaCaducidadRefreshToken(refreshTokenHash);

        if(!fechaExpiracionToken.Exitoso) throw fechaExpiracionToken.Excepcion ?? new Exception("Servicio no disponible en este momento");

        if(fechaExpiracionToken.Valor is null || fechaExpiracionToken.Valor <= DateTime.UtcNow) throw new CredencialesInvalidas();

        var usuarioId = await _repositorioAutenticacion.ObtenerUsuarioIdRefreshToken(refreshTokenHash);

        if(!usuarioId.Exitoso) throw usuarioId.Excepcion ?? new Exception("Servicio no disponible en este momento");

        if(usuarioId.Valor is null) throw new CredencialesInvalidas();

        var usuario = await _repositorioAutenticacion.ObtenerUsuarioId(usuarioId.Valor.Value);

        if(!usuario.Exitoso) throw usuario.Excepcion ?? new Exception("Servicio no disponible en este momento");

        if(usuario.Valor is null) throw new CredencialesInvalidas();

        var token = _jwtTokenServicio.GenerarAccessToken(usuario.Valor);
        var nuevoRefreshToken = _jwtTokenServicio.GenerarRefreshToken();

        if(!token.Exitoso) throw token.Excepcion ?? new Exception("Servicio no disponible en este momento");

        if(!nuevoRefreshToken.Exitoso) throw nuevoRefreshToken.Excepcion ?? new Exception("Servicio no disponible en este momento");

        var refreshTokenAnterior = new RefreshToken(refreshToken, refreshTokenHash, fechaExpiracionToken.Valor.Value);

        var actualizado = await _repositorioAutenticacion.ActualizarToken(refreshTokenAnterior, nuevoRefreshToken.Valor);

        if(!actualizado.Exitoso) throw actualizado.Excepcion ?? new Exception("Servicio no disponible en este momento");

        var usuarioDTO = new UsuarioDTO()
        {
            Nombre = usuario.Valor.Nombre,
            NombreUsuario = usuario.Valor.NombreUsuario,
            CorreoElectronico = usuario.Valor.CorreoElectronico.Valor,
            Rol = usuario.Valor.Rol.Nombre
        };

        var dto = new LoggedDTO()
        {
            token = token.Valor,
            refreshToken = nuevoRefreshToken.Valor,
            Usuario = usuarioDTO
        };
        return dto;
    }
    public async Task<UsuarioDTO> ObtenerUsuarioSesion(Guid usuarioId)
    {
        var usuario = await _repositorioAutenticacion.ObtenerUsuarioId(usuarioId);

        if(!usuario.Exitoso) throw usuario.Excepcion ?? new Exception("Servicio no disponible en este momento");

        if(usuario.Valor is null) throw new CredencialesInvalidas();

        var usuarioDTO = new UsuarioDTO()
        {
            Nombre = usuario.Valor.Nombre,
            NombreUsuario = usuario.Valor.NombreUsuario,
            CorreoElectronico = usuario.Valor.CorreoElectronico.Valor,
            Rol = usuario.Valor.Rol.Nombre
        };
        return usuarioDTO;
    }
}