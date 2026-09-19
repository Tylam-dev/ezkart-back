using Microsoft.AspNetCore.Mvc;

namespace Auth.Api;

public class AutenticacionController : ControllerBase
{
    private readonly IAutenticacionServicio _autenticacionServicio;
    public AutenticacionController(IAutenticacionServicio autenticacionServicio)
    {
        _autenticacionServicio = autenticacionServicio;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        try{
            var token = await _autenticacionServicio.LoginAsync(loginDTO);
            return Ok(new { Token = token });
        }
        catch (CredencialesInvalidas ex)
        {
            // Manejo de errores específicos, por ejemplo, credenciales inválidas
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            // Manejo de errores, por ejemplo, credenciales inválidas
            return StatusCode(500, new { Message = "Servicio no disponible" });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromHeader] int logoutDTO)
    {
        try
        {
            // Aquí iría la lógica para cerrar sesión del usuario utilizando el DTO recibido
            // Por ejemplo, podrías llamar a un servicio de autenticación que invalide el token JWT

            // Simulación de cierre de sesión exitoso
            return Ok(new { Message = "Sesión cerrada correctamente" });
        }
        catch (Exception ex)
        {
            // Manejo de errores, por ejemplo, usuario no encontrado
            return NotFound(new { Message = ex.Message });
        }
    }
}
