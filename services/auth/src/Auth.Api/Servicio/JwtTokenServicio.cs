using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Auth.Application.IServicios;
using Auth.Domain;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using Auth.Application.Utilidades;

namespace Auth.Api.Servicio;

public class JwtTokenServicio : IJwtTokenServicio
{
    private readonly string _key;
    private readonly double _expiracionMinutos;
    private readonly double _expiracionRefreshDias;
    private readonly string _issuer = "auth-api-service";
    private readonly string _audience = "ezkart-back-gateway";

    public JwtTokenServicio(string key, double expiracionMinutos, double expiracionRefreshDias)
    {
        _key = key;
        _expiracionMinutos = expiracionMinutos;
        _expiracionRefreshDias = expiracionRefreshDias;
    }

    public Resultado<TokenAcceso> GenerarAccessToken(Usuario usuario)
    {
        var manejador = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_expiracionMinutos),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _issuer,
            Audience = _audience
        };
        var token = manejador.CreateToken(tokenDescriptor);
        var tokenAcceso = new TokenAcceso(manejador.WriteToken(token), tokenDescriptor.Expires.Value);
        return Resultado<TokenAcceso>.Exito(tokenAcceso);
    }

    public Resultado<RefreshToken> GenerarRefreshToken()
    {
        var byteNumeroRandom = new byte[32];
        using (var numeroRandom = RandomNumberGenerator.Create())
        {
            numeroRandom.GetBytes(byteNumeroRandom);
            var refreshTokenValue = Convert.ToBase64String(byteNumeroRandom);
            var refreshTokenHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(refreshTokenValue)));
            var refreshToken = new RefreshToken(refreshTokenValue, refreshTokenHash, DateTime.UtcNow.AddDays(_expiracionRefreshDias));
            return Resultado<RefreshToken>.Exito(refreshToken);
        }
    }
}