public interface IAutenticacionServicio
{
    Task<LoginDTO> LoginAsync(LoginDTO loginDTO);
    Task LogoutAsync(Guid id);
}