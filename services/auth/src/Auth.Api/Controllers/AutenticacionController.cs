using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Auth.Application.IServicios;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api;

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
        try{
            var session = await _autenticacionServicio.LoginAsync(loginDTO);
            
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

        _logger.LogInformation("Usuario autenticado");

        return Ok();
    }

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
