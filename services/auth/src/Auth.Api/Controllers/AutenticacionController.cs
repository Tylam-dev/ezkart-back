using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Application.IServicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api;
[ApiController]
[Route("auth")]
public class AutenticacionController : ControllerBase
{
    private readonly string keyAccessToken = "access_token_ezkart";
    private readonly string keyRefreshToken = "refresh_token_ezkart";
    private readonly IAutenticacionServicio _autenticacionServicio;
    private readonly ILogger<AutenticacionController> _logger;
    public AutenticacionController(
        IAutenticacionServicio autenticacionServicio,
        ILogger<AutenticacionController> logger)
    {
        _autenticacionServicio = autenticacionServicio;
        _logger = logger;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        LoggedDTO? session;
        try{
            session = await _autenticacionServicio.LoginAsync(loginDTO);
        }
        catch (CredencialesInvalidas ex)
        {
            _logger.LogWarning(ex.Message);
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Inesperado", new { StackTrace = ex.StackTrace });
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }

        Response.Cookies.Append(keyAccessToken, session.token.Valor, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = session.token.Expiracion,
                Path = "/"
            });

            Response.Cookies.Append(keyRefreshToken, session.refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = session.refreshToken.Expiracion,
                Path = "/auth/refresh"
            });
        _logger.LogInformation("Usuario autenticado");

        return Ok(session.Usuario);
    }
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> ObtenerSesion()
    {
        try
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (usuarioId is null || !Guid.TryParse(usuarioId, out var usuarioGuid))
                return Unauthorized();

            var usuario = await _autenticacionServicio.ObtenerUsuarioSesion(usuarioGuid);

            _logger.LogInformation("Sesion vigente");

            return Ok(usuario);
        }
        catch (CredencialesInvalidas ex)
        {
            _logger.LogWarning(ex.Message);
            return Unauthorized(new { Message = ex.Message });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error Inesperado");
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }
    }
    [HttpGet("refresh")]
    public async Task<IActionResult> RefrescarToken()
    {
        try
        {
            var refreshToken = Request.Cookies[keyRefreshToken];
            if (refreshToken is null)
                return Unauthorized();

            var nuevaSession = await _autenticacionServicio.RefrescarToken(refreshToken);

            Response.Cookies.Append(keyAccessToken, nuevaSession.Valor, new CookieOptions
            {
               HttpOnly = true,
               Secure = true,
               SameSite = SameSiteMode.Lax,
               Expires = nuevaSession.Expiracion,
               Path = "/"
            });
        }
        catch (CredencialesInvalidas ex)
        {
            _logger.LogWarning(ex.Message);
            return Unauthorized(new { Message = ex.Message });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error Inesperado", new { StackTrace = ex.StackTrace });
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }

        _logger.LogInformation("Jwt Renovado");

        return Ok();
    }
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refresh = Request.Cookies[keyRefreshToken];

        if (refresh is not null)
        {
            var hashToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(refresh)));
            await _autenticacionServicio.LogoutAsync(hashToken);
        }

        Response.Cookies.Delete(keyAccessToken, new CookieOptions
        {
            Path = "/", Secure = true, SameSite = SameSiteMode.Lax
        });
        Response.Cookies.Delete(keyRefreshToken, new CookieOptions
        {
            Path = "/auth", Secure = true, SameSite = SameSiteMode.Lax
        });

        _logger.LogInformation("Cookies revocadas");

        return NoContent();
    }
}
