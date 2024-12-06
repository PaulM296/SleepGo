using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;
using SleepGo.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SleepGo.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly JwtSettings _settings;
        private readonly byte[] _key;

        public IdentityService(IOptions<JwtSettings> jwtOptions)
        {
            _settings = jwtOptions.Value;
            ArgumentNullException.ThrowIfNull(_settings);
            ArgumentNullException.ThrowIfNull(_settings.SigningKey);
            ArgumentNullException.ThrowIfNull(_settings.Audiences);
            ArgumentNullException.ThrowIfNull(_settings.Audiences[0]);
            ArgumentNullException.ThrowIfNull(_settings.Issuer);
            _key = Encoding.ASCII.GetBytes(_settings?.SigningKey!);
        }

        private static JwtSecurityTokenHandler TokenHandler => new();

        public ClaimsIdentity CreateClaimsIdentity(AppUser newUser)
        {
            return new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, newUser.UserName ?? throw new InvalidOperationException()),
                new Claim(JwtRegisteredClaimNames.Email, newUser.Email ?? throw new InvalidOperationException()),
                new Claim("userId", newUser.Id.ToString() ?? throw new InvalidOperationException()),
                new Claim(ClaimTypes.Role, newUser.Role.ToString() ?? throw new InvalidOperationException())
            });
        }

        public string CreateSecurityToken(ClaimsIdentity identity)
        {
            var tokenDescriptor = GetTokenDescriptor(identity);

            var tokenHandler = TokenHandler.CreateToken(tokenDescriptor);
            return TokenHandler.WriteToken(tokenHandler);
        }

        private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
        {
            return new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.Now.AddHours(6),
                Audience = _settings!.Audiences?[0],
                Issuer = _settings!.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
            };
        }
    }
}
