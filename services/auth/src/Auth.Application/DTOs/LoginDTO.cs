using System.ComponentModel.DataAnnotations;

public class LoginDTO
{
    [Required]
    public string NombreUsuario { get; set; } = null!;
    [Required]
    public string Contrasena { get; set; } = null!;
}