namespace JWT_AuthAndRefrest.Models.Custom
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
        public string ExpirationToken{ get; set; }
    }
}
