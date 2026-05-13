using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> GenerateToken(UserModel user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration.GetSection("JWT:Issuer").Value,
            _configuration.GetSection("JWT:Audience").Value,
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> LoginAsync(LoginDTO loginDTO)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }
        return await GenerateToken(user);
    }

    public async Task<string> ClientRegisterAsync(ClientSignUpDTO user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            throw new InvalidOperationException("Email already in use");
        }

        var newUser = new UserModel
        {
            Name = user.Name,
            Email = user.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Role = "Client"
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return await GenerateToken(newUser);
    }

    public async Task<string> UserRegisterAsync(UserSignUpDTO user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            throw new InvalidOperationException("Email already in use");
        }

        var newUser = new UserModel
        {
            Name = user.Name,
            Email = user.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Role = "Freelancer",
            SkillSet = user.SkillSet != null ? string.Join(",", user.SkillSet) : string.Empty

        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return await GenerateToken(newUser);
    }
}