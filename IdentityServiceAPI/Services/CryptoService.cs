using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityServiceAPI.Models;
using IdentityServiceAPI.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServiceAPI.Services
{
    public class CryptoService : ICryptoService
    {
        private readonly IConfiguration _config;

        private readonly byte[] _globalSaltBytes;
        private readonly byte[] _tokenSecurityKey;
        private const int LocalSaltLength = 256;
        private readonly TokenSettings _tokenSettings;
        private readonly TokenValidationParameters _validationParameters;

        public CryptoService(IConfiguration configuration)
        {
            _config = configuration;

            var globalSaltString = configuration["CryptoService:GlobalSalt"];
            var tokenSecurityKeyString = configuration["CryptoService:TokenSecurityToken"];

            if (globalSaltString == String.Empty || tokenSecurityKeyString == String.Empty)
            {
                //TODO
            }

            _globalSaltBytes = Encoding.UTF8.GetBytes(globalSaltString);
            _tokenSecurityKey = Encoding.UTF8.GetBytes(tokenSecurityKeyString);

            _tokenSettings = new TokenSettings();
            var tokenSettingsSection = configuration.GetSection("CryptoService").GetSection("TokenSettings");
            tokenSettingsSection.Bind(_tokenSettings);

            _validationParameters = new TokenValidationParameters()
            {
                ValidIssuer = _tokenSettings.Issuer,
                ValidAudience = _tokenSettings.Audience
            };

        }

        
        public Task<bool> VerifyPasswordAsync(byte[] expectedPassword, string passwordString, byte[] localSaltBytes)
        {
            return Task.Run(() => VerifyPassword(expectedPassword, passwordString, localSaltBytes));
        }

        private bool VerifyPassword(byte[] expectedPassword, string passwordString, byte[] localSaltBytes)
        {
            var passwordHash = GetPasswordHash(passwordString, localSaltBytes);

            return expectedPassword.SequenceEqual(passwordHash);
        }

        public byte[] GetLocalSaltBytes()
        {
            using var provider = RandomNumberGenerator.Create();

            byte[] saltBytes = new byte[LocalSaltLength];
            provider.GetBytes(saltBytes);

            return saltBytes;
        }

        public byte[] GetSHA256Hash(byte[] bytes)
        {
            var hashManager = SHA256.Create();
            return hashManager.ComputeHash(bytes);
        }

        public byte[] GetPasswordHash(string passwordString, byte[] localHashBytes)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(passwordString);
            var preHash = GetSHA256Hash(passwordBytes.AddBytes(_globalSaltBytes));

            return GetSHA256Hash(preHash.AddBytes(localHashBytes));
        }

        public string CreateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(_tokenSecurityKey);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var currentTime = DateTime.Now;
            var identity = GetIdentity(user);

            var token = new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Audience,
                notBefore: currentTime,
                claims: identity.Claims,
                expires: currentTime.AddMinutes(_tokenSettings.LifeTime),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_tokenSecurityKey), SecurityAlgorithms.HmacSha256)
            );

            var tokenString= new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public int VerifyToken(string token, string fullName)
        {
            var handler = new JwtSecurityTokenHandler();
            SecurityToken validateToken;
            var user = handler.ValidateToken(token, _validationParameters , out validateToken);

            if (user == null)
                return -1;
            if(user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value == fullName)
            {
                var roleClaim = user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role);
                if(roleClaim != null)
                    return Int32.Parse(roleClaim.Value);
            }


            return -1;
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.RoleName)
            };

            var identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return identity;
        }
    }
}
