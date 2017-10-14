using System;
using System.Security.Claims;
using System.Text;
using OauthService.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace OauthService.TokenProviders.Jwt
{
    public class JwtGenerator : ITokenGenerator
    {
        private readonly string _issuer;
        private readonly TimeSpan _tokenPeriod;

        public JwtGenerator(string issuer, TimeSpan period)
        {
            _issuer = issuer;
            _tokenPeriod = period;
        }

        public string Generate(Client client, ClaimsIdentity identity)
        {
            var signingCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(client.Secret)),
                    SecurityAlgorithms.HmacSha256);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Audience = client.Identifier,
                SigningCredentials = signingCredentials,
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now + _tokenPeriod,
                Issuer = _issuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

            return signedAndEncodedToken;
        }
    }
}
