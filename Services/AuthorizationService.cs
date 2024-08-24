using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWT_AuthAndRefrest.Models;
using JWT_AuthAndRefrest.Models.Custom;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using JWT_AuthAndRefrest.Context;
using System.Security.Cryptography;
using JWT_AuthAndRefrest.Models.Entities;


namespace JWT_AuthAndRefrest.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthorizationService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string GenerateToken(string idUser)
        {
            var key = _configuration.GetValue<string>("Jwt:key");
            var keyBytes = Encoding.UTF8.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUser));//idUser será la llave

            //Crear credencial para el token
            var creds = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature
                );

            var description = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = creds
            };

            //Controladores
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(description);

            string tokenCreated = tokenHandler.WriteToken(tokenConfig);

            return tokenCreated;
        }

        //REFRESH TOKEN

        private string GenerateRefreshToken()
        { 
            var byteArray = new byte[64];
            var refreshToken = "";

            using (var rng = RandomNumberGenerator.Create())
            { 
                rng.GetBytes(byteArray);
                refreshToken = Convert.ToBase64String(byteArray);
            }
            return refreshToken;
        }

        private async Task<AuthorizationResponse> SaveRefreshTokenRecord(int idUser, string token, string refreshToken)
        {
            var refreshTokenRecord = new RefreshToken
            {
                IdUser = idUser,
                Token = token,
                RefreshTokenS = refreshToken,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(2)
            };

            await _context.RefreshTokens.AddAsync(refreshTokenRecord);
            await _context.SaveChangesAsync();

            return new AuthorizationResponse { Token = token, RefrestToken = refreshToken, Result = true, Message = "Ok" };
        }


        public async Task<AuthorizationResponse> ReturnToken(AuthorizationRequest authorization)
        {
            var userFound = _context.Users.FirstOrDefault(x =>
                x.NameUser == authorization.NameUser &&
                x.Key == authorization.Key
            );

            if (userFound == null)
            {
                return await Task.FromResult<AuthorizationResponse>(null);
            }

            string tokenCreated = GenerateToken(userFound.Id.ToString());

            string refreshTokenCreated = GenerateRefreshToken();

            //return new AuthorizationResponse()
            //{
            //    Token = tokenCreated,
            //    Result = true,
            //    Message = "Ok"
            //};

            return await SaveRefreshTokenRecord(userFound.Id, tokenCreated, refreshTokenCreated);
        }

        public async Task<AuthorizationResponse> ReturnRefreshToken(RefreshTokenRequest refreshTokenRequest, int idUser)
        {
            var refreshTokenFound = _context.RefreshTokens.FirstOrDefault(x =>
                x.Token == refreshTokenRequest.ExpirationToken &&
                x.RefreshTokenS == refreshTokenRequest.RefreshToken &&//SDFSDFSFSD
                x.IdUser == idUser);

            if (refreshTokenFound == null)
                return new AuthorizationResponse 
                { 
                    Result = false, Message = "No existe refreshToken" 
                };
            
            var refreshTokenCreated = GenerateRefreshToken();
            var tokenCreated = GenerateToken(idUser.ToString());

            return await SaveRefreshTokenRecord(idUser, tokenCreated, refreshTokenCreated);
        }
    }
}
