using JWT_AuthAndRefrest.Models.Custom;

namespace JWT_AuthAndRefrest.Services
{
    public interface IAuthorizationService
    {
        Task<AuthorizationResponse> ReturnToken(AuthorizationRequest authorization);
        Task<AuthorizationResponse> ReturnRefreshToken(RefreshTokenRequest refreshTokenRequest, int idUser);
    }
}
