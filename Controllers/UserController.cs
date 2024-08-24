using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JWT_AuthAndRefrest.Models.Custom;
using JWT_AuthAndRefrest.Services;
using System.IdentityModel.Tokens.Jwt;

namespace JWT_AuthAndRefrest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public UserController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Route("Auth")]
        public async Task<IActionResult> Authentic([FromBody] AuthorizationRequest authorizationRequest)
        {
            var resultAuthorization = await _authorizationService.ReturnToken(authorizationRequest);
            if (resultAuthorization == null)
            {
                return Unauthorized();
            }

            return Ok(resultAuthorization);
        }


        [HttpPost]
        [Route("GetRefreshToken")]
        public async Task<IActionResult> GetRefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var possiblyExpiredToken = tokenHandler.ReadJwtToken(refreshTokenRequest.ExpirationToken);

            if (possiblyExpiredToken.ValidTo > DateTime.UtcNow)
                return BadRequest(new AuthorizationResponse { Result = false, Message = "Aún no ha expirado su Token" });

            string idUser = possiblyExpiredToken.Claims.First(x =>
                x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();

            var authorizationResponse = await _authorizationService.ReturnRefreshToken(refreshTokenRequest, int.Parse(idUser));

            if (authorizationResponse.Result)
                return Ok(authorizationResponse);
            else
                return BadRequest(authorizationResponse);
        }
    }
}
