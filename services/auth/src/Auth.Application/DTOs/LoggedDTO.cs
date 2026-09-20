public class LoggedDTO
{
    public TokenAcceso token { get; set; } = null!;
    public RefreshToken refreshToken {get;set;} = null!;
    public UsuarioDTO Usuario {get;set;}
}