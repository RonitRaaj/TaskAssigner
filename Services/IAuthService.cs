public interface IAuthService
{
    Task<string> GenerateToken(UserModel user);
    Task<string> LoginAsync(LoginDTO loginDTO);
    Task<string> ClientRegisterAsync(ClientSignUpDTO user);
    Task<string> UserRegisterAsync(UserSignUpDTO user);
}